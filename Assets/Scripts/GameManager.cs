using UnityEngine;
using UnityEngine.UI;

namespace ShapeReplica
{
    public class GameManager : MonoBehaviour//todo make a singleton
    {
        [SerializeField] private float previewDuration = 1;
        private float previewRemainTime;

        [SerializeField] Slider roundTimeIndicator;
        private Image indicatorImage;

        [SerializeField] private Button restartButton;
        [SerializeField] private Text gameOverText;
        [SerializeField] private Text RenaimingLivesText;

        [SerializeField] private Image gameOverPanel;

        public static bool IsPlaying;

        private int numCorrect;
        private int remainingLives;

        private RecognitionBoard recognitionBoard;
        private bool isShowPreview;
        [SerializeField]
        private int numLives = 3;

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
            ToggleGameOverPanel(false);
            recognitionBoard = GetComponent<RecognitionBoard>();
            RecognitionBoard.GestureRecognized += OnGestureRecognized;
            indicatorImage = roundTimeIndicator.GetComponentInChildren<Image>();
        }

        private void Pause()
        {
            IsPlaying = false;
            Time.timeScale = 0;//todo
        }

        public void Resume()
        {
            IsPlaying = true;
            Time.timeScale = 1;//todo
        }

        void Update ()
        {
            if (!IsPlaying) return;
            if(isShowPreview) { UpdateTimeIndicator();}
        }

        private void UpdateTimeIndicator()
        {
            previewRemainTime -= Time.deltaTime;
            roundTimeIndicator.value = previewRemainTime;

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
            ToggleGameOverPanel(!IsPlaying);
        }

        private void ToggleGameOverPanel(bool isVisible)
        {
            gameOverPanel.gameObject.SetActive(isVisible);
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
            roundTimeIndicator.maxValue = previewRemainTime = previewDuration;
        }

        public void StartGame()
        {
            RemainingLives = numLives;
            IsPlaying = true;
            isShowPreview = true;
            SetDrawingEnabled(true);
            ToggleGameOverPanel(!IsPlaying);//todo

            numCorrect = 0;
            ResetTimer();
            recognitionBoard.NextGesture();
        }

        private void SetDrawingEnabled(bool enable)
        {
            recognitionBoard.drawingBoard.enabled = enable;
        }

        public void OnShowMenuButtonClicked()
        {
            EventManager.TriggerEvent(EventCollection.OpenMenu);
        }
    }
}
