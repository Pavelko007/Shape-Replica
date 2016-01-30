using UnityEngine;

namespace RecognizeGesture
{
    public abstract class GestureDrawerBase : MonoBehaviour
    {
        public Transform StrokePrefab;
        public abstract void BeginNewStroke();
        public abstract void AddPoint(Vector2 touchPosition);
        public abstract void Clear();
    }
}