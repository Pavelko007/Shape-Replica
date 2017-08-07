using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ShapeReplica
{
    public class QuitTrigger : MonoBehaviour
    {
        public void Awake()
        {
            GetComponent<Button>().onClick.AddListener(
                () => EventManager.TriggerEvent(EventCollection.Quit));
        }
    }
}
