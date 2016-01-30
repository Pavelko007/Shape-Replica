using UnityEngine;

namespace RecognizeGesture
{
    public abstract class GestureDrawerBase : MonoBehaviour
    {
        public Transform StrokePrefab;
        public abstract void BeginNewStroke();
        public abstract void AddPoint(Vector2 touchPosition);
        public abstract void Clear();

        public GestureDrawerBase WithPrefab(Transform strokePrefab)
        {
            StrokePrefab = strokePrefab;
            return this;
        }

        protected T CreateNewLine<T>() where T: Component
        {
            return (Instantiate(StrokePrefab, Input.mousePosition, Quaternion.identity) as Transform)
                .GetComponent<T>();
        }
    }
}