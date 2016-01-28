using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

using PDollarGestureRecognizer;

namespace RecognizeGesture
{
    public class RecognitionBoard : DrawingBoard
    {
        private string message;
        protected List<Gesture> Gestures = new List<Gesture>();

        void Start()
        {
            Init();
            LoadGestures();
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
            Result gestureResult = PointCloudRecognizer.Classify(candidate, Gestures.ToArray());
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
            return recognized;
        }
    }
}
