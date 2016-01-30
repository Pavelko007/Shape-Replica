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
        

        public override void BeginNewStroke()
        {
            curLineRendererPoints.Clear();

            CurrentGestureLineRenderer = 
                (Instantiate(StrokePrefab, transform.position, transform.rotation) as Transform)
                .GetComponent<LineRenderer>();
            LineRenderers.Add(CurrentGestureLineRenderer);
        }

        public override void AddPoint(Vector2 touchPos)
        {
            Vector3 lineRendererPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
            curLineRendererPoints.Add(lineRendererPoint);
            CurrentGestureLineRenderer.SetVertexCount(curLineRendererPoints.Count);
            CurrentGestureLineRenderer.SetPositions(curLineRendererPoints.ToArray());
        }

        public override void Clear()
        {
            curLineRendererPoints.Clear();
            foreach (LineRenderer lineRenderer in LineRenderers)
            {
                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }
            LineRenderers.Clear();
        }
    }
}