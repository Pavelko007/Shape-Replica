using UnityEngine;

namespace ShapeReplica.GestureDrawer
{
    public class TrailDrawer : GestureDrawerBase
    {
        private TrailRenderer trailRenderer;

        public override bool IsDrawing
        {
            get { return trailRenderer != null; }
        }

        public override void BeginNewStroke()
        {
            trailRenderer = CreateNewLine<TrailRenderer>();
        }

        public override void AddPoint(Vector2 touchPosition)
        {
            trailRenderer.transform.position = ConvertPointPos(touchPosition);
        }

        public override void Clear()
        {
            if (null == trailRenderer) return;
            Destroy(trailRenderer.gameObject);
        }
    }
}