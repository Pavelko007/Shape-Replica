using System;
using UnityEngine;
using UnityEngine.Events;

namespace ShapeReplica.EventSystem
{
    [Serializable]
    public class UnityEventBool : UnityEvent<bool> { }

    public class GameEventListenerBool : MonoBehaviour, IEventListener<bool>
    {
        public GameEventBool Event;
        public UnityEventBool Response;

        public void OnEnable()
        {
            Event.RegisterListener(this);
        }

        public void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised(bool value)
        {
            Response.Invoke(value);
        }
    }
}