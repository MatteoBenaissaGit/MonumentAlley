using System;
using DG.Tweening;
using MatteoBenaissaLibrary.AudioManager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button _button;
        [SerializeField] private SoundEnum _hoverSound;
        [SerializeField] private SoundEnum _clickSound;

        private bool _isClicked;

        private void Awake()
        {
            _button.onClick.AddListener(Click);
        }

        private void Click()
        {
            _isClicked = true;
            transform.DOComplete();
            transform.DOMoveX(transform.position.x + 50, 1f);
            SoundManager.Instance.PlaySound(_clickSound);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isClicked) return;
            
            _text.DOKill();
            _text.DOFade(1f, 0.3f);
            SoundManager.Instance.PlaySound(_hoverSound);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isClicked) return;

            _text.DOKill();
            _text.DOFade(0.2f, 0.3f);
        }

        public void SetIsClicked() => _isClicked = true;
    }
}