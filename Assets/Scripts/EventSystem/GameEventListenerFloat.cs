using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShapeReplica.EventSystem
{
    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    public class GameEventListenerFloat : MonoBehaviour
    {
        public GameEventFloat Event;
        public UnityEventFloat Response; 

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(float value)
        {
            Response.Invoke(value);
        }
    }
}