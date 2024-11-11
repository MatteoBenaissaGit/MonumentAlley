using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TouchEffect : MonoBehaviour
    {
        [SerializeField] private Image _touchCircle;
        [SerializeField] private float _touchCircleSize;
        [SerializeField] private SpriteRenderer _touchCircleSprite;
        [SerializeField] private float _touchCircleSpriteSize;
        
        private void Awake()
        {
            _touchCircle.color = new Color(1f, 1f, 1f, 0f);
            _touchCircleSprite.color = new Color(1f, 1f, 1f, 0f);
        }

        public void Play(Vector3 point, Vector3 up)
        {
            _touchCircleSprite.transform.forward = up;
            
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(point);
            _touchCircle.transform.position = screenPoint;
            _touchCircleSprite.transform.position = point + Vector3.up * 0.025f;

            _touchCircle.transform.DOKill();
            _touchCircle.transform.localScale = Vector3.zero;
            _touchCircle.transform.DOScale(_touchCircleSize, 0.5f).SetEase(Ease.OutExpo);
            _touchCircle.DOKill();
            _touchCircle.color = Color.white;
            _touchCircle.DOFade(0, 0.5f).SetEase(Ease.InQuint);

            _touchCircleSprite.transform.DOKill();
            _touchCircleSprite.transform.localScale = Vector3.one * _touchCircleSpriteSize;
            _touchCircleSprite.transform.DOScale(Vector3.one * (_touchCircleSpriteSize + 0.2f), 0.15f).SetEase(Ease.OutCubic)
                .OnComplete(() =>  _touchCircleSprite.transform.DOScale(Vector3.one * (_touchCircleSpriteSize-0.1f), 0.5f).SetEase(Ease.OutExpo));
            _touchCircleSprite.DOKill();
            _touchCircleSprite.color = Color.white;
            _touchCircleSprite.DOColor(Color.black, 0.4f).SetEase(Ease.InQuart)
                .OnComplete(() => _touchCircleSprite.DOFade(0, 0.5f).SetEase(Ease.InCirc)
                    .OnComplete(() => Destroy(gameObject)));
            
        }
    }
}