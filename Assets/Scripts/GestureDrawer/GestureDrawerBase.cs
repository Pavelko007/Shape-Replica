using UnityEngine;

namespace ShapeReplica.GestureDrawer
{
    public abstract class GestureDrawerBase : MonoBehaviour
    {
        public Transform StrokePrefab;

        public abstract bool IsDrawing { get; }

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

        protected Vector3 ConvertPointPos(Vector2 touchPos)
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(touchPos.x, touchPos.y, 10));
        }
    }
}