using System;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.UI;

namespace ShapeReplica
{
    class GestureEditor : MonoBehaviour
    {
        [SerializeField] private DrawingBoard drawingBoard;
        [SerializeField] private InputField inputField;

        private string shapeTitle = "";

        public void SetShapeTitle(string newTitle)
        {
            shapeTitle = newTitle;
        }

        public void AddNewGesture()
        {
            if (drawingBoard.DrawingPoints.Count <= 0 || shapeTitle == "") return;

            string fileName = string.Format("{0}/{1}-{2}.xml",
                Application.persistentDataPath,
                shapeTitle,
                DateTime.Now.ToFileTime());

#if !UNITY_WEBPLAYER
            GestureIO.WriteGesture(drawingBoard.DrawingPoints.ToArray(), shapeTitle, fileName);
#endif
            inputField.text = "";
        }
    }
}