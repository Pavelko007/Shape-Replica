using System.Linq;
using PDollarGestureRecognizer;
using UnityEngine;

namespace ShapeReplica
{
    public class GestureRenderer : MonoBehaviour
    {
        public Transform gestureOnScreenPrefab;
        private LineRenderer currentGestureLineRenderer;
        [SerializeField]
        private RectTransform PanelRect;

        public void RenderGesture(Gesture nextGesture)
        {
            if (currentGestureLineRenderer != null) Destroy(currentGestureLineRenderer.gameObject);

            currentGestureLineRenderer = (Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform)
                .GetComponent<LineRenderer>();

            var rendererPoints = nextGesture.Points
                .Select(gesturePoint => new Vector3(gesturePoint.X, -gesturePoint.Y, 0))
                .ToArray();

            currentGestureLineRenderer.SetVertexCount(rendererPoints.Length);
            currentGestureLineRenderer.SetPositions(rendererPoints);

            TransformGesture();
        }

        private void TransformGesture()
        {
            currentGestureLineRenderer.useWorldSpace = false;
            Transform LRTransform = currentGestureLineRenderer.gameObject.transform;

            //set scale
            Vector3 scale = LRTransform.localScale;
            scale.x = scale.y = CalcShapeScale();
            LRTransform.localScale = scale;

            //set position
            LRTransform.position = PanelRect.position + Vector3.back;
        }

        private float CalcShapeScale()
        {
            float panelSize = (PanelRect.TransformPoint(Vector3.one * PanelRect.rect.xMax)
                               - PanelRect.TransformPoint(Vector3.one * PanelRect.rect.xMin)).x;

            float shapeSize = Mathf.Max(currentGestureLineRenderer.bounds.size.x,
                currentGestureLineRenderer.bounds.size.y);

            float fillCoef = .7f;
            float scaleMult = fillCoef * panelSize / shapeSize;
            return scaleMult;
        }
    }
}
