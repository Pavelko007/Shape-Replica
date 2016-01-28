using System;
using PDollarGestureRecognizer;
using UnityEngine;

namespace RecognizeGesture
{
    class GestureEditor : DrawingBoard
    {
        protected string newGestureName = "";
        private bool gestureAdded = false;

        void Start()
        {
            Init();
        }

        protected void ProcessAddNewGesture()
        {
            GUI.Label(new Rect(Screen.width - 200, 150, 70, 30), "Add as: ");
            newGestureName = GUI.TextField(new Rect(Screen.width - 150, 150, 100, 30), newGestureName);

            if (GUI.Button(new Rect(Screen.width - 50, 150, 50, 30), "Add") && points.Count > 0 && newGestureName != "")
            {
                gestureAdded = true;
                string fileName = String.Format("{0}/{1}-{2}.xml", Application.persistentDataPath, newGestureName,
                    DateTime.Now.ToFileTime());

#if !UNITY_WEBPLAYER
                GestureIO.WriteGesture(points.ToArray(), newGestureName, fileName);
#endif
                newGestureName = "";
            }
        }

        void OnGUI()
        {
            base.OnGUI();
            ProcessAddNewGesture();
        }

        protected override bool ShouldCleanBoard()
        {
            return gestureAdded;
        }
    }
}