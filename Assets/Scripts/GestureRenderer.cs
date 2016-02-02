using UnityEngine;
using System.Linq;
using PDollarGestureRecognizer;

public class GestureRenderer : MonoBehaviour
{
    public Transform gestureOnScreenPrefab;
    private LineRenderer currentGestureLineRenderer;
    [SerializeField] private RectTransform PanelRect;

    public void RenderGesture(Gesture nextGesture)
    {
        if(currentGestureLineRenderer != null) Destroy(currentGestureLineRenderer.gameObject);

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

        var scale = LRTransform.localScale;
        scale.x = scale.y = 3;
        LRTransform.localScale = scale;

        LRTransform.position = PanelRect.position + Vector3.back;
    }
}
