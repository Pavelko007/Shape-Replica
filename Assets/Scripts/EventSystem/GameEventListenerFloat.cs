using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShapeReplica.EventSystem
{
    [Serializable]
    public class UnityEventFloat : UnityEvent<float> { }

    public class GameEventListenerFloat : MonoBehaviour, IEventListener<float>
    {
        public GameEventFloat Event;
        public UnityEventFloat Response;

        public void OnEnable()
        {
            Event.RegisterListener(this);
        }

        public void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(float value)
        {
            Response.Invoke(value);
        }
    }
}