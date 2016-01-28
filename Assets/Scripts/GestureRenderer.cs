using UnityEngine;
using System.Linq;
using PDollarGestureRecognizer;

public class GestureRenderer : MonoBehaviour
{
    public Transform gestureOnScreenPrefab;
    private LineRenderer currentGestureLineRenderer;

    // Use this for initialization
    void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("GestureSet/10-stylus-MEDIUM/10-stylus-medium-T-01");
        RenderGesture(GestureIO.ReadGestureFromXML(textAsset.text));
    }

    public void RenderGesture(Gesture nextGesture)
    {
        if(currentGestureLineRenderer != null) Destroy(currentGestureLineRenderer.gameObject);

        currentGestureLineRenderer = (Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform)
            .GetComponent<LineRenderer>();

        var rendererPoints = nextGesture.Points
            .Select(gesturePoint => new Vector3(gesturePoint.X, -gesturePoint.Y, 10))
            .ToArray();

        currentGestureLineRenderer.SetVertexCount(rendererPoints.Length);
        currentGestureLineRenderer.SetPositions(rendererPoints);

        TransformGesture();
    }

    private void TransformGesture()
    {
        currentGestureLineRenderer.useWorldSpace = false;
        Transform LRTransform = currentGestureLineRenderer.gameObject.transform;
        LRTransform.localScale *= 3;
        var screenPoint = new Vector2(Screen.width * 4 / 5f, Screen.height * 1 / 2f);
        LRTransform.Translate(Camera.main.ScreenToWorldPoint(screenPoint));
    }
}
