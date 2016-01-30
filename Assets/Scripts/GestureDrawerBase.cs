using UnityEngine;

namespace RecognizeGesture
{
    public abstract class GestureDrawerBase : MonoBehaviour
    {
        public abstract void BeginNewStroke(Transform trailRendererPrefab);
    }
}