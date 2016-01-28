﻿using UnityEngine;
using System.Linq;
using PDollarGestureRecognizer;

public class GestureRenderer : MonoBehaviour
{
    public Gesture Gesture;
    public Transform gestureOnScreenPrefab;
    private LineRenderer currentGestureLineRenderer;

    // Use this for initialization
    void Start()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("GestureSet/10-stylus-MEDIUM/10-stylus-medium-T-01");
        Gesture = GestureIO.ReadGestureFromXML(textAsset.text);
        Transform tmpGesture = Instantiate(gestureOnScreenPrefab, transform.position, transform.rotation) as Transform;
        currentGestureLineRenderer = tmpGesture.GetComponent<LineRenderer>();

        var rendererPoints = Gesture.Points
            .Select(gesturePoint => new Vector3(gesturePoint.X, -gesturePoint.Y, 10))
            .ToArray();

        currentGestureLineRenderer.SetVertexCount(rendererPoints.Length);
        currentGestureLineRenderer.SetPositions(rendererPoints);
    }
}
