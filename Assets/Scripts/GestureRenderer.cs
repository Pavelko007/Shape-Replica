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
            scale.x = scale.y = GetShapeScale();
            LRTransform.localScale = scale;

            //set position
            LRTransform.position = PanelRect.position + Vector3.back;
        }

        private float GetShapeScale()
        {
            float fillCoef = .8f;
            return fillCoef * GetPanelSize() / GetShapeSize();
        }

        private float GetShapeSize()
        {
            return currentGestureLineRenderer.bounds.size.magnitude;
        }

        private float GetPanelSize()
        {
            Vector3 maxPoint = PanelRect.TransformPoint(PanelRect.rect.max);
            Vector3 minPoint = PanelRect.TransformPoint(PanelRect.rect.min);
            return (maxPoint - minPoint).magnitude;
        }
    }
}
