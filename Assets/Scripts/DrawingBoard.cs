using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;

namespace RecognizeGesture
{
    public abstract class DrawingBoard : MonoBehaviour
    {
        public bool IsLinesDisappear = false;

        public Transform LineRenderPrefab;
        public Transform TrailRendererPrefab;

        protected List<Point> points = new List<Point>();

        protected int strokeId = -1;

        protected Vector2 TouchPosition = Vector2.zero;
        protected Rect drawArea;

        protected RuntimePlatform platform;

        public double RecognitionThreshold;

        private TrailDrawer trailDrawer;
        private LineDrawer lineDrawer;
        private GestureDrawerBase gestureDrawer;

        protected virtual void Awake()
        {
            if (IsLinesDisappear)
            {
                trailDrawer = gameObject.AddComponent(typeof (TrailDrawer)) as TrailDrawer;
                trailDrawer.StrokePrefab = TrailRendererPrefab;
                gestureDrawer = trailDrawer;
            }
            else
            {
                lineDrawer = gameObject.AddComponent(typeof(LineDrawer)) as LineDrawer;
                lineDrawer.StrokePrefab = LineRenderPrefab;
                gestureDrawer = lineDrawer;
            }
        }

        void Update()
        {
            UpdateTouchPos();

            if (!drawArea.Contains(TouchPosition)) return;

            if (Input.GetMouseButtonDown(0)) AddNewStroke();

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

            gestureDrawer.AddPoint(TouchPosition);
        }

        private void AddNewStroke()
        {
            ++strokeId;
            gestureDrawer.BeginNewStroke();
        }

        protected void CleanDrawingArea()
        {
            strokeId = -1;

            points.Clear();

            gestureDrawer.Clear();
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