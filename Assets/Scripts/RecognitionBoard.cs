using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;
using Random = UnityEngine.Random;

namespace RecognizeGesture
{
    public class RecognitionBoard : DrawingBoard
    {
        private string message;
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

        protected override void Awake()
        {
            base.Awake();
            gestureRenderer = GetComponent<GestureRenderer>();
            Init();
            LoadGestures();
        }
        
        public void NextGesture()
        {
            curGesture = Gestures[Random.Range(0, Gestures.Count)];
            recognitionStatus = RecognitionStatus.Await;
            gestureRenderer.RenderGesture(curGesture);
            CleanDrawingArea();
        }

        void OnGUI()
        {
            base.OnGUI();
            DrawMessage();
            ProcessRecognize();
        }

        private void ProcessRecognize()
        {
            var recognizeButtonRect = new Rect(Screen.width - 100, 10, 100, 30);
            if (GUI.Button(recognizeButtonRect, "Recognize")) TryRecognizeGesture();
        }

        private void TryRecognizeGesture()
        {
            Gesture candidate = new Gesture(points.ToArray());
            Result gestureResult = PointCloudRecognizer.Classify(candidate, new[] { curGesture });

            Debug.Log(string.Format("recognition score :{0}", gestureResult.Score));

            if (gestureResult.Score < RecognitionThreshold)
            {
                recognitionStatus = RecognitionStatus.Fail;
                message = "Gesture doesn't match. Try again";
                CleanDrawingArea();
            }
            else
            {
                recognitionStatus = RecognitionStatus.Recognized;
                message = gestureResult.GestureClass + " " + gestureResult.Score;
                GestureRecognized();
            }
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

        protected override bool ShouldCleanBoard()
        {
            return RecognitionStatus.Recognized == recognitionStatus ||
                RecognitionStatus.Fail == recognitionStatus;
        }
    }
}
