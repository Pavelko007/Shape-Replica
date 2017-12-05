using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShapeReplica.EventSystem
{
    [Serializable]
    public class EventBase<T> : ScriptableObject
    {
        private List<IEventListener<T>> listeners = new List<IEventListener<T>>();

        public void Raise(T value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void RegisterListener(IEventListener<T> listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(IEventListener<T> listener)
        {
            listeners.Remove(listener);
        }
    }
}