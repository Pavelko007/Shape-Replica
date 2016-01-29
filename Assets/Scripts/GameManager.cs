using RecognizeGesture;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float RoundLength = 10f;
    private float curRoundTimeLeft;
    private int score = 0;
    private bool isPlaying = true;
    
    //UI
    public Slider RoundTimeIndicator;
    public Button restartButton;

    private RecognitionBoard recognitionBoard;

    void Awake()
    {
        SetButtonVisibility(false);
        recognitionBoard = GetComponent<RecognitionBoard>();
        RecognitionBoard.GestureRecognized += OnGestureRecognized;

        //fit slider in screen
        RoundTimeIndicator.GetComponent<RectTransform>().sizeDelta = new Vector2(20, Screen.height * .9f);
    }

    private void SetButtonVisibility(bool isVisible)
    {
        restartButton.gameObject.SetActive(isVisible);
    }

    // Use this for initialization
	void Start ()
	{
        ResetRound();
        recognitionBoard.NextGesture();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!isPlaying) return;

	    curRoundTimeLeft -= Time.deltaTime;
	    RoundTimeIndicator.value = curRoundTimeLeft;
	    if (curRoundTimeLeft < 0)
	    {
	        Debug.Log(string.Format("Time elapsed, your score is {0} points", score));
	        isPlaying = false;
	        SetButtonVisibility(true);
	    }
	}

    void OnGestureRecognized()
    {
        score++;
        ResetRound();
        recognitionBoard.NextGesture();
    }

    private void ResetRound()
    {
      
        RoundTimeIndicator.maxValue = RoundLength;
        curRoundTimeLeft = RoundLength;

        RoundLength *= .95f;
    }

    public void RestartGame()
    {
        SetButtonVisibility(false);
        isPlaying = true;
        ResetRound();
        recognitionBoard.NextGesture();
    }
}
