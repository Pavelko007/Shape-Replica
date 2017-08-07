using UnityEngine;
using UnityEngine.UI;

namespace ShapeReplica.Events
{
    public class PauseTrigger : MonoBehaviour
    {
        public void Awake()
        {
            GetComponent<Button>().onClick.AddListener(
                () => EventManager.TriggerEvent(EventCollection.Pause));
        }
    }
}
