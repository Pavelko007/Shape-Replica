using System;
using System.Collections.Generic;
using System.IO;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ShapeReplica
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

        void Update()
        {
            base.Update();
            if (Input.GetMouseButtonUp(0) && IsTouchPosOnBoard())
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
            CleanDrawingArea();
        }

        void OnGUI()
        {
            DrawMessage();
        }

        private void DrawMessage()
        {
            var messageRect = new Rect(10, Screen.height - 40, 500, 50);
            GUI.Label(messageRect, message);
        }

        public void CompareShapes()
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
