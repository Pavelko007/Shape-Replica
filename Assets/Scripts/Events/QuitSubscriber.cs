using UnityEngine;

namespace ShapeReplica
{
    public class QuitSubscriber : MonoBehaviour {

        void Awake ()
        {
            EventManager.StartListening(EventCollection.Quit, () =>
            {
                GetComponent<QuitApplication>().Quit();
            });
        }
    }
}
