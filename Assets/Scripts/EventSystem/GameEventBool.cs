using System.Collections.Generic;
using UnityEngine;

namespace ShapeReplica.EventSystem
{
    [CreateAssetMenu(fileName = "NewGameEventBool.asset", menuName = "GameEventBool", order = 1)]
    public class GameEventBool : ScriptableObject
    {
        private List<GameEventListenerBool> listeners = new List<GameEventListenerBool>();

        public void Raise(bool value)
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventRaised(value);
            }
        }

        public void RegisterListener(GameEventListenerBool listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListenerBool listener)
        {
            listeners.Remove(listener);
        }
    }
}