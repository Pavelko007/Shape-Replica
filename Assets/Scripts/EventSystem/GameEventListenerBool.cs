using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShapeReplica.EventSystem
{
    [Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    public class GameEventListenerBool : MonoBehaviour
    {
        public GameEventBool Event;
        public UnityEventBool Response; 

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(bool value)
        {
            Response.Invoke(value);
        }
    }
}