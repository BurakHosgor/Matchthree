using DG.Tweening;
using Events;
using UnityEngine;
using Utils;
using Zenject;

namespace UI.MainMenu
{
    public abstract class BaseNewGameButton : UIBTN
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private AudioClip _buttonClip;

        [Inject] private SoundManager _soundManager;

        private Sequence _clickSizeSeq;

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_clickSizeSeq.IsActive()) _clickSizeSeq.Kill();
        }

        protected override void OnClick()
        {
            _soundManager.PlaySoundEffect(_buttonClip);
            
            _transform.localScale = Vector3.one;

            _clickSizeSeq = DOTween.Sequence();

            Tween sizeIncTwn = _transform.DOScale(Vector3.one * 1.1f, 0.5f);
            sizeIncTwn.SetEase(Ease.OutElastic);

            Tween sizeDcrTwn = _transform.DOScale(Vector3.one, 0.5f);
            sizeDcrTwn.SetEase(Ease.OutElastic);

            _clickSizeSeq.Append(sizeIncTwn);
            _clickSizeSeq.Append(sizeDcrTwn);

            Tween secCounterTween = DOVirtual.Float(
                0,
                0.5f,
                0.5f,
                delegate (float e)
                {
                    // You can add logic here if needed
                });

            _clickSizeSeq.Append(secCounterTween);

            _clickSizeSeq.onComplete += OnButtonClickComplete;
        }

        protected abstract void OnButtonClickComplete();
    }
}