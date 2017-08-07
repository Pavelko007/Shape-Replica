using ShapeReplica;
using UnityEngine;

public class MenuButtonClickedSubscriber : MonoBehaviour
{
    [SerializeField]
    private ShowPanels showPanels;

	void Awake ()
	{
	    EventManager.StartListening(
            EventCollection.OpenMenu, 
            ()=>showPanels.ShowMenu());
	}
}
