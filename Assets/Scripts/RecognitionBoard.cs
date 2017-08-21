using System;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ShapeReplica
{
    public class RecognitionBoard : MonoBehaviour
    {
        [SerializeField] public DrawingBoard drawingBoard;
        [SerializeField] private double RecognitionThreshold = 0.7;
        [SerializeField] private Text statusText;

        public static event Action<bool> GestureRecognized;

        private List<Gesture> gestures = new List<Gesture>();
        private GestureRenderer gestureRenderer;
        private Gesture curGesture;
        private RecognitionStatus recognitionStatus;

        enum RecognitionStatus
        {
            Await,
            Recognized,
            Fail
        }

        void Awake()
        {
            gestureRenderer = GetComponent<GestureRenderer>();
            LoadGestures();
            statusText.text = "";
        }

        void Update()
        {
            if (!GameManager.IsPlaying) return;
        }

        public void NextGesture()
        {
            if (gestures.Count == 0) Debug.LogError("not enough gestures in library");

            PickAnotherRandomGesture();

            recognitionStatus = RecognitionStatus.Await;
            gestureRenderer.RenderGesture(curGesture);
            SetPreviewVisible(true);
            drawingBoard.CleanDrawingArea();
        }

        public void SetPreviewVisible(bool isVisible)
        {
            gestureRenderer.currentGestureLineRenderer.enabled = isVisible;
        }

        private void PickAnotherRandomGesture()
        {
            Gesture prevGesture = curGesture;

            do curGesture = gestures[Random.Range(0, gestures.Count)];
            while (curGesture == prevGesture);
        }

        public void CompareShapes()
        {
            if (!drawingBoard.IsDrawing) return;

            Gesture candidate = new Gesture(drawingBoard.DrawingPoints.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, new[] { curGesture });

            string statusString;
            //statusString = "Shapes match on " + (int)(gestureResult.Score * 100) + " %." + Environment.NewLine;

            if (gestureResult.Score < RecognitionThreshold)
            {
                statusString = "incorrect";
                recognitionStatus = RecognitionStatus.Fail;
                //statusString += "Threshold is " + (int)(RecognitionThreshold * 100) + " %. Try again";
                drawingBoard.CleanDrawingArea();
                GestureRecognized(false);
            }
            else
            {
                statusString = "correct";
                recognitionStatus = RecognitionStatus.Recognized;
                
                GestureRecognized(true);
            }
            statusText.text = statusString;
        }

        private void LoadGestures()
        {
            LoadPreMadeGestures();
            //LoadUserCustomGestures();
        }

        private void LoadUserCustomGestures()
        {
            string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
            foreach (string filePath in filePaths)
            {
                gestures.Add(GestureIO.ReadGestureFromFile(filePath));
            }
        }

        private void LoadPreMadeGestures()
        {

            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Shapes/");
            foreach (TextAsset gestureXml in gesturesXml)
            {
                gestures.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
            }
        }
    }
}
