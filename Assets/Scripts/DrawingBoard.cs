using System.Collections.Generic;
using PDollarGestureRecognizer;
using ShapeReplica.GestureDrawer;
using UnityEngine;

namespace ShapeReplica
{
    public abstract class DrawingBoard : MonoBehaviour
    {
        public bool IsLinesDisappear = false;

        public Transform LineRenderPrefab;
        public Transform TrailRendererPrefab;

        protected List<Point> points = new List<Point>();

        protected int strokeId = -1;

        protected Vector2 TouchPosition = Vector2.zero;

        protected RuntimePlatform platform;

        public double RecognitionThreshold;
        
        private GestureDrawerBase gestureDrawer;

        [SerializeField] private RectTransform boardRect;

        protected virtual void Awake()
        {
            gestureDrawer = IsLinesDisappear
                ? AddDrawer<TrailDrawer>(TrailRendererPrefab)
                : AddDrawer<LineDrawer>(LineRenderPrefab);
        }

        public void Update()
        {
            UpdateTouchPos();

            if (!Input.GetMouseButton(0) || !IsTouchPosOnBoard()) return;

            if (Input.GetMouseButtonDown(0)) AddNewStroke();

            AddGesturePoint();
        }

        public bool IsTouchPosOnBoard()
        {
            bool isTouchPosOnBoard = RectTransformUtility.RectangleContainsScreenPoint(boardRect, TouchPosition, Camera.main);
            return isTouchPosOnBoard;
        }

        private GestureDrawerBase AddDrawer<T>(Transform strokePrefab) where T : GestureDrawerBase
        {
            return (gameObject.AddComponent(typeof(T)) as T).WithPrefab(strokePrefab);
        }

        public void Init()
        {
            platform = Application.platform;
        }

        protected abstract bool ShouldCleanBoard();
        
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