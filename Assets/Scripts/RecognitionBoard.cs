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
        [SerializeField] private DrawingBoard drawingBoard;
        [SerializeField] private double RecognitionThreshold = 0.7;
        [SerializeField] private Text statusText;

        protected List<Gesture> Gestures = new List<Gesture>();
        private GestureRenderer gestureRenderer;
        private Gesture curGesture;
        private RecognitionStatus recognitionStatus;

        public static event Action GestureRecognized;

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
            
            if (Input.GetMouseButtonUp(0) &&
                drawingBoard.IsDrawing)
            {
                CompareShapes();
            }
        }

        public void NextGesture()
        {
            if (Gestures.Count <= 1) Debug.LogError("not enough gestures in library");

            Gesture newGesture;
            do newGesture = Gestures[Random.Range(0, Gestures.Count)];
            while (curGesture == newGesture);

            curGesture = newGesture;
            recognitionStatus = RecognitionStatus.Await;
            gestureRenderer.RenderGesture(curGesture);
            drawingBoard.CleanDrawingArea();
        }

        public void CompareShapes()
        {
            Gesture candidate = new Gesture(drawingBoard.points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, new[] { curGesture });

            Debug.Log(string.Format("recognition score :{0}", gestureResult.Score));


            string statusString = "Shapes match on " + (int) (gestureResult.Score * 100) + " %." + Environment.NewLine;

            if (gestureResult.Score < RecognitionThreshold)
            {
                recognitionStatus = RecognitionStatus.Fail;
                statusString += "Threshold is " + (int)(RecognitionThreshold * 100) + " %. Try again";
                drawingBoard.CleanDrawingArea();
            }
            else
            {
                recognitionStatus = RecognitionStatus.Recognized;
                statusString += "You got it.";
                GestureRecognized();
            }
            statusText.text = statusString;
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
                Gestures.Add(GestureIO.ReadGestureFromFile(filePath));
            }
        }

        private void LoadPreMadeGestures()
        {
            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("Gestures/");
            foreach (TextAsset gestureXml in gesturesXml)
            {
                Gestures.Add(GestureIO.ReadGestureFromXML(gestureXml.text));
            }
        }
    }
}
