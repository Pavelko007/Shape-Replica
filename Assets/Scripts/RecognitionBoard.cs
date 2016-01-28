using UnityEngine;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

namespace RecognizeGesture
{
    public class RecognitionBoard : DrawingBoard
    {
        private string message;
        protected List<Gesture> Gestures = new List<Gesture>();
        private GestureRenderer gestureRenderer;
        private Gesture curGesture;
        private RecognitionStatus recognitionStatus;

        enum RecognitionStatus
        {
            Await,
            Recognized,
            Fail
        }

        void Start()
        {
            Init();
            LoadGestures();

            gestureRenderer = GetComponent<GestureRenderer>();
            NextGesture();
        }

        private void NextGesture()
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
            Result gestureResult = PointCloudRecognizer.Classify(candidate, new []{curGesture});

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
                NextGesture();
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
            TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/10-stylus-MEDIUM/");
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
