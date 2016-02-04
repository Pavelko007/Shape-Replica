using System;
using PDollarGestureRecognizer;
using UnityEngine;

namespace ShapeReplica
{
    class GestureEditor : MonoBehaviour
    {
        [SerializeField] private DrawingBoard drawingBoard;

        private string newGestureName = "";
        
        protected void ProcessAddNewGesture()
        {
            var gestureNameLabelRect = new Rect(Screen.width - 200, 150, 70, 30);
            GUI.Label(gestureNameLabelRect, "Add as: ");

            var gestureNameTextFieldRect = new Rect(Screen.width - 150, 150, 100, 30);
            newGestureName = GUI.TextField(gestureNameTextFieldRect, newGestureName);

            var addGestureRect = new Rect(Screen.width - 50, 150, 50, 30);

            if (GUI.Button(addGestureRect, "Add") &&
                drawingBoard.DrawingPoints.Count > 0 &&
                newGestureName != "")
            {
                AddNewGesture();
            }
        }

        private void AddNewGesture()
        {
            string fileName = string.Format("{0}/{1}-{2}.xml",
                Application.persistentDataPath,
                newGestureName,
                DateTime.Now.ToFileTime());

#if !UNITY_WEBPLAYER
            GestureIO.WriteGesture(drawingBoard.DrawingPoints.ToArray(), newGestureName, fileName);
#endif
            newGestureName = "";
        }

        void OnGUI()
        {
            ProcessAddNewGesture();
        }

      
    }
}