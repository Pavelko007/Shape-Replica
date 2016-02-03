using System.Collections.Generic;
using PDollarGestureRecognizer;
using ShapeReplica.GestureDrawer;
using UnityEngine;

namespace ShapeReplica
{
    public class DrawingBoard : MonoBehaviour
    {
        public bool IsLinesDisappear = false;

        public Transform LineRenderPrefab;
        public Transform TrailRendererPrefab;

        public List<Point> points = new List<Point>();

        protected int strokeId = -1;

        protected Vector2 TouchPosition = Vector2.zero;

        protected RuntimePlatform platform;

        private GestureDrawerBase gestureDrawer;

        private RectTransform boardRect;

        protected virtual void Awake()
        {
            Init();
        }

        public void Update()
        {
            if (!GameManager.IsPlaying) return;

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
            boardRect = GetComponent<RectTransform>();
            platform = Application.platform;

            gestureDrawer = IsLinesDisappear
               ? AddDrawer<TrailDrawer>(TrailRendererPrefab)
               : AddDrawer<LineDrawer>(LineRenderPrefab);
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

        public void CleanDrawingArea()
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