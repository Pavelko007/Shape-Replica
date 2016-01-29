using RecognizeGesture;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float RoundLength = 10f;
    private float curRoundTimeLeft;
    private int score = 0;
    private bool isPlaying = true;
    
    public Slider RoundTimeIndicator;

    private RecognitionBoard recognitionBoard;

    void Awake()
    {
        recognitionBoard = GetComponent<RecognitionBoard>();
        RecognitionBoard.GestureRecognized += OnGestureRecognized;
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

        RoundLength *= 0.95f;
    }
}
