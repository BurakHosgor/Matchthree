using Events;
using UnityEngine;
using Utils;
using Zenject;
using DG.Tweening;
using UnityEngine.UI;

namespace UI.MainMenu
{
    public class MainMenuManager : EventListenerMono
    {
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _settingsMenuPanel;
        [Inject] private SoundManager _soundManager;
        
        [SerializeField] private Image _menuHeaderImage;
        [SerializeField] private Image _levelHeaderImage;
        [SerializeField] private Image _settingsHeaderImage;
        [SerializeField] private float _floatDuration = 2f;
        [SerializeField] private float _floatDistance = 10f;
        

        [SerializeField] private AudioClip _soundButton;
        
        private Tween _menuHeaderTween;
        private Tween _levelHeaderTween;
        private Tween _settingsHeaderTween;

        private void Start()
        {
            SetPanelActive(_mainMenuPanel);
            StartFloating();
        }
        
        private void StartFloating()
        {
            Vector3 originalPosition = _menuHeaderImage.rectTransform.anchoredPosition;
            Vector3 originalPosition1 = _levelHeaderImage.rectTransform.anchoredPosition;
            Vector3 originalPosition2 = _settingsHeaderImage.rectTransform.anchoredPosition;

            // Move up
           _menuHeaderTween = _menuHeaderImage.rectTransform.DOAnchorPosY(originalPosition.y + _floatDistance, _floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
           _levelHeaderTween = _levelHeaderImage.rectTransform.DOAnchorPosY(originalPosition1.y + _floatDistance, _floatDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
           _settingsHeaderTween = _settingsHeaderImage.rectTransform.DOAnchorPosY(originalPosition2.y + _floatDistance, _floatDuration)
               .SetEase(Ease.InOutSine)
               .SetLoops(-1, LoopType.Yoyo);
            
        }
        private void OnDestroy()
        {
            // Tween'leri yok et
            _menuHeaderTween?.Kill();
            _levelHeaderTween?.Kill();
            _settingsHeaderTween?.Kill();
        }
        private void SetPanelActive(GameObject panel)
        {
            _mainMenuPanel.SetActive(_mainMenuPanel == panel);
            _settingsMenuPanel.SetActive(_settingsMenuPanel == panel);
        }
        
        protected override void RegisterEvents()
        {
            MainMenuEvents.SettingsBTN += OnSettingsBTN;
            MainMenuEvents.SettingsExitBTN += OnSettingsExitBTN;
        }

        private void OnSettingsBTN()
        {
            SetPanelActive(_settingsMenuPanel);
            _soundManager.PlaySoundEffect(_soundButton);
        }

        private void OnSettingsExitBTN()
        {
            SetPanelActive(_mainMenuPanel);
            _soundManager.PlaySoundEffect(_soundButton);
        }

        protected override void UnRegisterEvents()
        {
            MainMenuEvents.SettingsBTN -= OnSettingsBTN;
            MainMenuEvents.SettingsExitBTN -= OnSettingsExitBTN;
        }
    }
}