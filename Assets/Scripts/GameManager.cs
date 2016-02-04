using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ShapeReplica
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private float RoundLength = 10f;
        private float curRoundLenth;
        private float curRoundTimeLeft;

        [SerializeField] Slider roundTimeIndicator;
        private Image indicatorImage;

        [SerializeField] private Button restartButton;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Image gameOverPanel;

        public static bool IsPlaying;

        private int score;

        private RecognitionBoard recognitionBoard;

        void Awake()
        {
            ToggleGameOverPanel(false);
            recognitionBoard = GetComponent<RecognitionBoard>();
            RecognitionBoard.GestureRecognized += OnGestureRecognized;
            indicatorImage = roundTimeIndicator.GetComponentInChildren<Image>();
        }

        void Start ()
        {
            RestartGame();
        }

        void Update ()
        {
            if (!IsPlaying) return;

            UpdateTimeIndicator();
        }

        private void UpdateTimeIndicator()
        {
            curRoundTimeLeft -= Time.deltaTime;
            roundTimeIndicator.value = curRoundTimeLeft;

            float timeLeftNorm = curRoundTimeLeft / curRoundLenth;
            if (timeLeftNorm > 2 / 3f) indicatorImage.color = Color.blue;
            else if (timeLeftNorm > 1 / 3f) indicatorImage.color = Color.yellow;
            else if (timeLeftNorm > 0) indicatorImage.color = Color.red;
            else GameOver();
        }

        private void GameOver()
        {
            gameOverText.text = string.Format("Time elapsed, you scored {0} points", score);
            IsPlaying = false;
            ToggleGameOverPanel(true);
        }

        private void ToggleGameOverPanel(bool isVisible)
        {
            gameOverPanel.gameObject.SetActive(isVisible);
        }

        void OnGestureRecognized()
        {
            score++;
            ResetRound();
            recognitionBoard.NextGesture();
        }

        private void ResetRound()
        {
            roundTimeIndicator.maxValue = curRoundTimeLeft = curRoundLenth;

            curRoundLenth *= .85f;
        }

        public void RestartGame()
        {
            ToggleGameOverPanel(false);
            IsPlaying = true;
            score = 0;
            curRoundLenth = RoundLength;
            ResetRound();
            recognitionBoard.NextGesture();
        }
    }
}
