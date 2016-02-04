using System;
using System.Collections;
using System.Linq;
using PDollarGestureRecognizer;
using UnityEngine;

namespace ShapeReplica
{
    public class GestureRenderer : MonoBehaviour
    {
        [SerializeField] private Transform gestureOnScreenPrefab;
        [SerializeField] private RectTransform panelRect;

        private LineRenderer currentGestureLineRenderer;


        public void RenderGesture(Gesture nextGesture)
        {
            if (currentGestureLineRenderer != null) Destroy(currentGestureLineRenderer.gameObject);

            currentGestureLineRenderer = Instantiate(gestureOnScreenPrefab).GetComponent<LineRenderer>();

            Vector3[] rendererPoints = nextGesture.Points
                .Select(gesturePoint => new Vector3(gesturePoint.X, -gesturePoint.Y, 0))
                .ToArray();

            currentGestureLineRenderer.SetVertexCount(rendererPoints.Length);
            currentGestureLineRenderer.SetPositions(rendererPoints);

            StartCoroutine("TransformGesture");
        }
       
        private IEnumerator TransformGesture()
        {
            // wait until Sample Panel has resized by vectical layout group
            while (Math.Abs(GetPanelSize()) < float.Epsilon)
            {
                yield return new WaitForEndOfFrame();
            }

            currentGestureLineRenderer.useWorldSpace = false;

            Transform shapeTransform = currentGestureLineRenderer.transform;

            Vector3 scale = shapeTransform.localScale;
            scale.x = scale.y = GetShapeScale();
            shapeTransform.localScale = scale;

            shapeTransform.position = panelRect.position + Vector3.back;
        }

        private float GetShapeScale()
        {
            float fillCoef = .8f;
            return fillCoef * GetPanelSize() / GetShapeSize();
        }

        private float GetShapeSize()
        {
            Vector3 shapeSize = currentGestureLineRenderer.bounds.size;
            return Mathf.Max(shapeSize.x, shapeSize.y);
        }

        private float GetPanelSize()
        {
            Rect rect = panelRect.rect;
            return panelRect.TransformVector(rect.max - rect.min).x;
        }
    }
}
