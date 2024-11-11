using System;
using DG.Tweening;
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
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isClicked) return;
            
            _text.DOKill();
            _text.DOFade(1f, 0.3f);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isClicked) return;

            _text.DOKill();
            _text.DOFade(0.2f, 0.3f);
        }
    }
}