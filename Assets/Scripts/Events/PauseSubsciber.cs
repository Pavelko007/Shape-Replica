using ShapeReplica;
using UnityEngine;

public class PauseSubsciber : MonoBehaviour {
	void Awake()
	{
	    var pause = GetComponent<Pause>();

	    EventManager.StartListening(EventCollection.Pause, () => pause.TogglePause()); 
	}
	
	 
}
