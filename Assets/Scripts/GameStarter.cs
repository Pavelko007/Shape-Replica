using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour {

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
