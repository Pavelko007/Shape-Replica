using System.Collections.Generic;
using UnityEngine;

namespace RecognizeGesture
{
    class LineDrawer : GestureDrawerBase
    {
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();
        private LineRenderer curLineRenderer;

        private List<Vector3> curLineRendererPoints = new List<Vector3>();

        public override void BeginNewStroke()
        {
            curLineRendererPoints.Clear();

            curLineRenderer = CreateNewLine<LineRenderer>();
            lineRenderers.Add(curLineRenderer);
        }

        public override void AddPoint(Vector2 touchPos)
        {
            Vector3 lineRendererPoint = Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
            curLineRendererPoints.Add(lineRendererPoint);
            curLineRenderer.SetVertexCount(curLineRendererPoints.Count);
            curLineRenderer.SetPositions(curLineRendererPoints.ToArray());
        }

        public override void Clear()
        {
            curLineRendererPoints.Clear();
            foreach (LineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.SetVertexCount(0);
                Destroy(lineRenderer.gameObject);
            }
            lineRenderers.Clear();
        }
    }
}