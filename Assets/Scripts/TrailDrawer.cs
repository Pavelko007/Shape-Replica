using System.Collections.Generic;
using UnityEngine;

namespace RecognizeGesture
{
    public class TrailDrawer : GestureDrawerBase
    {
        public TrailRenderer curTrailRenderer;
        public List<TrailRenderer> trailRenderers = new List<TrailRenderer>();

        public override void BeginNewStroke(Transform trailRendererPrefab)
        {
            curTrailRenderer = (Instantiate(trailRendererPrefab, Input.mousePosition, Quaternion.identity) as Transform)
                    .GetComponent<TrailRenderer>();
            trailRenderers.Add(curTrailRenderer);
        }

        public void AddPoint(Vector2 touchPosition)
        {
            if (curTrailRenderer != null)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(touchPosition);
                mousePos.z = 10;
                curTrailRenderer.transform.position = mousePos;
            }
        }

        public void Clear()
        {
            foreach (TrailRenderer trailRenderer in trailRenderers)
            {
                Destroy(trailRenderer.gameObject);
            }
            trailRenderers.Clear();
        }
    }
}