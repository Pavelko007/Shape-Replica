using ShapeReplica.EventSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ShapeReplica
{
    public class GameManager : MonoBehaviour//todo make a singleton
    {
        [SerializeField] private float previewDuration = 1;
        private float previewRemainTime;
        [SerializeField] Slider elapsedTimeSlider;
        private Image indicatorImage;

        [SerializeField] private Button restartButton;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Text RenaimingLivesText;

        [SerializeField] private GameObject gameOverPanel;

        public static bool IsPlaying;

        private int numCorrect;
        private int remainingLives;

        private RecognitionBoard recognitionBoard;
        private bool isShowPreview;
        [SerializeField]
        private int numLives = 3;

        private float previewRemainTimeNorm;//todo

        public int RemainingLives
        {
            get
            {
                return remainingLives;
            }
            set
            {
                if (value <= 0)
                {
                    GameOver();
                }
                remainingLives = value;
                RenaimingLivesText.text = string.Format("you have {0} lives left", remainingLives);
            }
        }

        void Awake()
        {
            gameOverPanel.SetActive(false);
            recognitionBoard = GetComponent<RecognitionBoard>();
            RecognitionBoard.GestureRecognized += OnGestureRecognized;
        }

        public void OnGamePaused(bool isPaused)
        {
            if (isPaused)
            {
                Pause();
            }
            else Resume();
        }

        public void Pause()
        {
            IsPlaying = false;
            recognitionBoard.drawingBoard.enabled = false;
            Time.timeScale = 0;//todo
        }

        public void Resume()
        {
            IsPlaying = true;
            recognitionBoard.drawingBoard.enabled = true;

            Time.timeScale = 1;//todo
        }

        void Update ()
        {
            if (!IsPlaying) return;
            if(isShowPreview) { UpdateTimeIndicator();}
        }

        [SerializeField] private GameEventFloat onShowCountdownTimeNorm;

        private void UpdateTimeIndicator()
        {
            previewRemainTime -= Time.deltaTime;
            previewRemainTimeNorm = previewRemainTime / previewDuration;
            onShowCountdownTimeNorm.Raise(previewRemainTimeNorm);

            if (previewRemainTime < 0)
            {
                isShowPreview = false;
                recognitionBoard.SetPreviewVisible(false);
            }
        }

        private void GameOver()//todo
        {
            IsPlaying = false;

            gameOverText.text = string.Format("You scored {0} points", numCorrect);
            recognitionBoard.drawingBoard.enabled = IsPlaying;
            gameOverPanel.SetActive(true);
        }

        void OnGestureRecognized(bool isCorrect)
        {
            ResetTimer();
            isShowPreview = true;
            recognitionBoard.NextGesture();
            if (isCorrect)
            {
                numCorrect++;
            }
            else
            {
                RemainingLives--;
            }
        }

        private void ResetTimer()
        {
            previewRemainTimeNorm = 1;
            previewRemainTime = previewDuration;
        }

        public void StartGame()
        {
            RemainingLives = numLives;
            IsPlaying = true;
            isShowPreview = true;
            SetDrawingEnabled(true);
            gameOverPanel.SetActive(false);

            numCorrect = 0;
            ResetTimer();
            recognitionBoard.NextGesture();
        }

        private void SetDrawingEnabled(bool enable)
        {
            recognitionBoard.drawingBoard.enabled = enable;
        }
    }
}
