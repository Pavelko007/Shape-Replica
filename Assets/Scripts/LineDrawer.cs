using System.Collections.Generic;
using UnityEngine;

namespace RecognizeGesture
{
    class LineDrawer : GestureDrawerBase
    {
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();

        public List<LineRenderer> LineRenderers
        {
            set { lineRenderers = value; }
            get { return lineRenderers; }
        }

        public LineRenderer currentGestureLineRenderer = null;

        public LineRenderer CurrentGestureLineRenderer
        {
            get { return currentGestureLineRenderer; }
            set { currentGestureLineRenderer = value; }
        }

        protected List<Vector3> curLineRendererPoints = new List<Vector3>();

        public List<Vector3> CurLineRendererPoints
        {
            set { curLineRendererPoints = value; }
            get { return curLineRendererPoints; }
        }

        public override void BeginNewStroke()
        {
            if (currentGestureLineRenderer != null) CurLineRendererPoints.Clear();

            Transform tmpGesture =
                Instantiate(StrokePrefab, transform.position, transform.rotation) as Transform;
            CurrentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();
            LineRenderers.Add(CurrentGestureLineRenderer);
        }

        public override void AddPoint(Vector2 touchPos)
        {
            Vector3 lineRendererPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
            CurLineRendererPoints.Add(lineRendererPoint);
            CurrentGestureLineRenderer.SetVertexCount(CurLineRendererPoints.Count);
            CurrentGestureLineRenderer.SetPositions(CurLineRendererPoints.ToArray());
        }

        public override void Clear()
        {
            CurLineRendererPoints.Clear();
            foreach (LineRenderer lineRenderer in LineRenderers)
            {
                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }
            LineRenderers.Clear();
        }
    }
}