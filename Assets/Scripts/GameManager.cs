using RecognizeGesture;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float RoundLength = 10f;
    private float curRoundLenth;
    private float curRoundTimeLeft;

    private int score;

    private bool isPlaying;

    private RecognitionBoard recognitionBoard;

    //UI
    [SerializeField]
    private Slider RoundTimeIndicator;

    [SerializeField]
    private Button restartButton;

    private Image indicatorImage;

    void Awake()
    {
        SetButtonVisibility(false);
        recognitionBoard = GetComponent<RecognitionBoard>();
        RecognitionBoard.GestureRecognized += OnGestureRecognized;
        indicatorImage = RoundTimeIndicator.GetComponentInChildren<Image>();
    }

    void Start ()
    {
        RestartGame();
    }

    void Update ()
	{
	    if (!isPlaying) return;

	    curRoundTimeLeft -= Time.deltaTime;
	    RoundTimeIndicator.value = curRoundTimeLeft;

        float timeLeftNorm = curRoundTimeLeft / curRoundLenth;
        if (timeLeftNorm > 2 / 3f) indicatorImage.color = Color.blue;
        else if (timeLeftNorm > 1 / 3f) indicatorImage.color = Color.yellow;
        else if (timeLeftNorm > 0) indicatorImage.color = Color.red;
        else
        {
            Debug.Log(string.Format("Time elapsed, your score is {0} points", score));
            isPlaying = false;
            SetButtonVisibility(true);
        }
	}

    private void SetButtonVisibility(bool isVisible)
    {
        restartButton.gameObject.SetActive(isVisible);
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
