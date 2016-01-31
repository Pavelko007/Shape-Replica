using RecognizeGesture;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float RoundLength = 10f;
    private float curRoundLenth;
    private float curRoundTimeLeft;
    private int score;
    private bool isPlaying;
    
    //UI
    public Slider RoundTimeIndicator;
    public Button restartButton;

    private RecognitionBoard recognitionBoard;

    void Awake()
    {
        SetButtonVisibility(false);
        recognitionBoard = GetComponent<RecognitionBoard>();
        RecognitionBoard.GestureRecognized += OnGestureRecognized;
    }

    // Use this for initialization
    void Start ()
    {
        RestartGame();
    }

    private void SetButtonVisibility(bool isVisible)
    {
        restartButton.gameObject.SetActive(isVisible);
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
        RoundTimeIndicator.maxValue = curRoundLenth;
        curRoundTimeLeft = curRoundLenth;

        curRoundLenth *= .85f;
    }

    public void RestartGame()
    {
        SetButtonVisibility(false);
        isPlaying = true;
        score = 0;
        curRoundLenth = RoundLength;
        ResetRound();
        recognitionBoard.NextGesture();
    }
}
