using UnityEngine;
using UnityEngine.Events;

namespace ShapeReplica.EventSystem
{
    public class GameEventListener : MonoBehaviour, IEventListener
    {
        public GameEvent Event;
        public UnityEvent Response;//todo extend for arguments

        public void OnEnable()
        {
            Event.RegisterListener(this);
        }

        public void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}