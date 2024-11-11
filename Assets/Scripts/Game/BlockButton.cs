using DG.Tweening;
using MatteoBenaissaLibrary.AudioManager;
using UnityEngine;

namespace Game
{
    public class BlockButton : MonoBehaviour
    {
        [SerializeField] private EventSystem.Event _event;

        private bool _isClicked;

        public void Click()
        {
            if (_isClicked) return;
            
            GameManager.Instance.InputsActive = false;
            _isClicked = true;

            transform.DOComplete();
            transform.DOMoveY(transform.position.y - 0.3f, 1f);
            
            SoundManager.Instance?.PlaySound(SoundEnum.select, 0.05f);

            _event.Launch();
            _event.OnEnd += () => GameManager.Instance.InputsActive = true;
        }
    }
}