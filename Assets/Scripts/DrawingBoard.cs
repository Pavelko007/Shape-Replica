using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;

namespace RecognizeGesture
{
    public abstract class DrawingBoard : MonoBehaviour
    {
        public Transform gestureOnScreenPrefab;

        protected List<Point> points = new List<Point>();

        protected int strokeId = -1;

        protected Vector2 TouchPosition = Vector2.zero;
        protected Rect drawArea;

        protected RuntimePlatform platform;
        private List<LineRenderer> gestureLinesRenderer = new List<LineRenderer>();
        protected LineRenderer currentGestureLineRenderer = null;

        protected bool recognized;
        protected List<Vector3> curLineRendererPoints = new List<Vector3>();
        public double RecognitionThreshold;

        void Update()
        {
            UpdateTouchPos();

            if (!drawArea.Contains(TouchPosition)) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (ShouldCleanBoard()) CleanDrawingArea();

                AddNewStroke();
            }

            if (Input.GetMouseButton(0)) AddGesturePoint();
        }

        protected void OnGUI()
        {
            CalcDrawArea();
            GUI.Box(drawArea, "Draw Area");
        }

        public void Init()
        {
            platform = Application.platform;
            CalcDrawArea();
        }

        protected abstract bool ShouldCleanBoard();

        protected void CalcDrawArea()
        {
            float drawingAreaWidthFraction = 2 / 3f;
            drawArea = new Rect(0, 0, Screen.width * drawingAreaWidthFraction, Screen.height);
        }

        protected void AddGesturePoint()
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

        protected void UpdateTouchPos()
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