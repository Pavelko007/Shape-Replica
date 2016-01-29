using System.Collections.Generic;
using PDollarGestureRecognizer;
using UnityEngine;
using UnityEngine.Serialization;

namespace RecognizeGesture
{
    public abstract class DrawingBoard : MonoBehaviour
    {
        public bool IsLinesDisappear = false;

        public Transform LineRenderPrefab;
        public Transform TrailRendererPrefab;

        private TrailRenderer curTrailRenderer;
        private List<TrailRenderer> trailRenderers = new List<TrailRenderer>();

        protected List<Point> points = new List<Point>();

        protected int strokeId = -1;

        protected Vector2 TouchPosition = Vector2.zero;
        protected Rect drawArea;

        protected RuntimePlatform platform;
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();
        protected LineRenderer currentGestureLineRenderer = null;

        protected List<Vector3> curLineRendererPoints = new List<Vector3>();
        public double RecognitionThreshold;


        void Update()
        {
            UpdateTouchPos();

            if (!drawArea.Contains(TouchPosition)) return;

            if (Input.GetMouseButtonDown(0))
            {
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

            if (!IsLinesDisappear) AddLineRendererPoint();
            else if (curTrailRenderer != null)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(TouchPosition);
                mousePos.z = 10;
                curTrailRenderer.transform.position = mousePos;
            }
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

            if (IsLinesDisappear)
            {
                curTrailRenderer = (Instantiate(TrailRendererPrefab, Input.mousePosition, Quaternion.identity) as Transform).GetComponent<TrailRenderer>();
                trailRenderers.Add(curTrailRenderer);
            }
            else
            {
                Transform tmpGesture =
                    Instantiate(LineRenderPrefab, transform.position, transform.rotation) as Transform;
                currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
                lineRenderers.Add(currentGestureLineRenderer);
            }
        }

        protected void CleanDrawingArea()
        {
            strokeId = -1;

            points.Clear();
            curLineRendererPoints.Clear();

            if (IsLinesDisappear)
            {
                foreach (TrailRenderer trailRenderer in trailRenderers)
                {
                    Destroy(trailRenderer.gameObject);
                }
                trailRenderers.Clear();
            }
            else
            {
                foreach (LineRenderer lineRenderer in lineRenderers)
                {
                    lineRenderer.SetVertexCount(0);
                    Destroy(lineRenderer.gameObject);
                }
                lineRenderers.Clear();
            }
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