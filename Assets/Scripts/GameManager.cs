using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

namespace RecognizeGesture
{
    public class GameManager : MonoBehaviour
    {

        public Transform gestureOnScreenPrefab;

        private List<Gesture> trainingSet = new List<Gesture>();

        private List<Point> points = new List<Point>();
        private int strokeId = -1;

        private Vector2 TouchPosition = Vector2.zero;
        private Rect drawArea;

        private RuntimePlatform platform;

        private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
        private LineRenderer currentGestureLineRenderer = null;

        //GUI
        private string message;
        private bool recognized;
        private string newGestureName = "";
        private List<Vector3> curLineRendererPoints = new List<Vector3>();
        public double RecognitionThreshold;

        void Start()
        {
            platform = Application.platform;
            CalcDrawArea();
            LoadGestures();
        }

        void Update()
        {
            UpdateTouchPos();

            if (!drawArea.Contains(TouchPosition)) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (recognized) CleanDrawingArea();

                AddNewStroke();
            }

            if (Input.GetMouseButton(0)) AddGesturePoint();
        }

        void OnGUI()
        {
            CalcDrawArea();
            GUI.Box(drawArea, "Draw Area");
            DrawMessage();
            ProcessRecognize();
            ProcessAddNewGesture();
        }

        private void ProcessRecognize()
        {
            var recognizeButtonRect = new Rect(Screen.width - 100, 10, 100, 30);
            if (GUI.Button(recognizeButtonRect, "Recognize")) TryRecognizeGesture();
        }

        private void ProcessAddNewGesture()
        {
            GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
            newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

            if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "")
            {
                string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName,
                    DateTime.Now.ToFileTime());

#if !UNITY_WEBPLAYER
                GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
#endif

                trainingSet.Add(new Gesture(points.ToArray(), newGestureName));

                newGestureName = "";
            }
        }

        private void TryRecognizeGesture()
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
            recognized = true;

            Debug.Log(string.Format("recognition score :{0}", gestureResult.Score));
            
            message = gestureResult.Score < RecognitionThreshold
                ? "Gesture doesn't match. Try again"
                : gestureResult.GestureClass + " " + gestureResult.Score;
        }

        private void DrawMessage()
        {
            var messageRect = new Rect(10, Screen.height - 40, 500, 50);
            GUI.Label(messageRect, message);
        }

        private void LoadGestures()
        {
            LoadPreMadeGestures();
            LoadUserCustomGestures();
        }

        private void LoadUserCustomGestures()
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
            foreach (string filePath in filePaths)
            {
                trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));
            }
        }

        private void LoadPreMadeGestures()
        {
            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
            foreach (TextAsset gestureXml in gesturesXml)
            {
                trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
            }
        }

        private void CalcDrawArea()
        {
            float drawingAreaWidthFraction = 2 / 3f;
            drawArea = new Rect(0, 0, Screen.width * drawingAreaWidthFraction, Screen.height);
        }

        private void AddGesturePoint()
        {
            var point = new Point(TouchPosition.x, -TouchPosition.y, strokeId);
            points.Add(point);
            
            AddLineRendererPoint();
        }

        private void AddLineRendererPoint()
        {
            Vector3 lineRendererPoint = Camera.main.ScreenToWorldPoint(new Vector3(TouchPosition.x, TouchPosition.y, 10));
            curLineRendererPoints.Add(lineRendererPoint);
            currentGestureLineRenderer.SetVertexCount(curLineRendererPoints.Count);
            currentGestureLineRenderer.SetPositions(curLineRendererPoints.ToArray());
        }

        private void AddNewStroke()
        {
            if (currentGestureLineRenderer != null)
            {
                curLineRendererPoints.Clear();
            }
            ++strokeId;

            Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
            currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

            gestureLinesRenderer.Add(currentGestureLineRenderer);
        }

        private void CleanDrawingArea()
        {
            recognized = false;
            strokeId = -1;

            points.Clear();

            foreach (LineRenderer lineRenderer in gestureLinesRenderer)
            {
                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }

            gestureLinesRenderer.Clear();
        }

        private void UpdateTouchPos()
        {
            if (platform == RuntimePlatform.Android || platform == RuntimePlatform.IPhonePlayer)
            {
                if (Input.touchCount > 0)
                {
                    TouchPosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    TouchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                }
            }
        }
    }
}
