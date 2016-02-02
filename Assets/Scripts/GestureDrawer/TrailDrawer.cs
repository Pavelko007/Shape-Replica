using System.Collections.Generic;
using UnityEngine;

namespace RecognizeGesture.GestureDrawer
{
    public class TrailDrawer : GestureDrawerBase
    {
        public TrailRenderer curTrailRenderer;
        public List<TrailRenderer> trailRenderers = new List<TrailRenderer>();

        public override void BeginNewStroke()
        {
            curTrailRenderer = CreateNewLine<TrailRenderer>();
            trailRenderers.Add(curTrailRenderer);
        }

        public override void AddPoint(Vector2 touchPosition)
        {
            if (curTrailRenderer != null)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(touchPosition);
                mousePos.z = Camera.main.transform.position.z + 1;
                curTrailRenderer.transform.position = mousePos;
            }
        }

        public override void Clear()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }
    }
}