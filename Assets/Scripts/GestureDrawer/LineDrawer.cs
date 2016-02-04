using System.Collections.Generic;
using UnityEngine;

namespace ShapeReplica.GestureDrawer
{
    class LineDrawer : GestureDrawerBase
    {
        private LineRenderer lineRenderer;

        private List<Vector3> lineRendererPoints = new List<Vector3>();

        public override bool IsDrawing
        {
            get { return lineRenderer != null; }
        }

        public override void BeginNewStroke()
        {
            lineRendererPoints.Clear();

            lineRenderer = CreateNewLine<LineRenderer>();
        }

        public override void AddPoint(Vector2 touchPos)
        {
            lineRendererPoints.Add(ConvertPointPos(touchPos));

            lineRenderer.SetVertexCount(lineRendererPoints.Count);
            lineRenderer.SetPositions(lineRendererPoints.ToArray());
        }

        public override void Clear()
        {
            lineRendererPoints.Clear();

            if (!lineRenderer) return;
            lineRenderer.SetVertexCount(0);
            Destroy(lineRenderer.gameObject);
        }
    }
}