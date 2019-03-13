using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AstroNet.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _menuCanvasGroup;
        [SerializeField] private CanvasGroup _gameCanvasGroup;
        [SerializeField] private float _fadeTime;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _returnButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _continueButton;

        [SerializeField] private TMP_Text _score;

        public Action OnStartButtoClicked;
        public Action OnReturnButtoClicked;
        public Action OnPauseButtoClicked;
        public Action OnContinueButtonClicked;

        private Coroutine _fadeCorotuine;
        // Start is called before the first frame update
        void Start()
        {
            _startButton.onClick.AddListener(StartButtonClick);
            _returnButton.onClick.AddListener(ReturnButtonClick);
            _pauseButton.onClick.AddListener(PauseButtonClick);
            _continueButton.onClick.AddListener(ContinueButtonClick);
        }

        private void StartButtonClick()
        {
            Debug.Log("Start button clicked");
            OnStartButtoClicked?.Invoke();
        }

        private void ReturnButtonClick()
        {
            Debug.Log("Return button clicked");
            OnReturnButtoClicked?.Invoke();
        }

        private void PauseButtonClick()
        {
            Debug.Log("Pause button clicked");
            ShowPauseButton(false);
            OnPauseButtoClicked?.Invoke();
        }
        private void ContinueButtonClick()
        {
            Debug.Log("Continue button clicked");
            ShowPauseButton(true);
            OnContinueButtonClicked?.Invoke();
        }

        public void ShowMenu(bool show, float fadeTime = 0.5f)
        {
            ShowCanvas(_menuCanvasGroup, show, fadeTime);
        }

        public void ShowGameCanvas(bool show, float fadeTime = 0.5f)
        {
            ShowCanvas(_gameCanvasGroup, show, fadeTime);
            ShowPauseButton(true);
        }
        private void ShowButton(Button button, bool show)
        {
            button.gameObject.SetActive(show);
        }
        private void ShowPauseButton(bool show)
        {
            ShowButton(_pauseButton, show);
            ShowButton(_continueButton, !show);
        }
        public void SetScore(float score)
        {
            _score.text = ((int)score).ToString();
        }
        private void ShowCanvas(CanvasGroup canvasGroup, bool show, float fadeTime)
        {
            canvasGroup.blocksRaycasts = show;

            canvasGroup.alpha = show ? 1f : 0f;
        }
        private IEnumerator FadeCanvasGroup(CanvasGroup group, bool show, float time)
        {
            var wait = new WaitForEndOfFrame();
            var step = 1f / time;
            if (show)
            {
                group.alpha = 0f;
                while (group.alpha < 1f)
                {
                    group.alpha += step;
                    yield return wait;
                }
            }
            else
            {
                group.alpha = 1f;
                while (group.alpha > 0f)
                {
                    group.alpha -= step;
                    yield return wait;
                }
            }
        }
    }
}
