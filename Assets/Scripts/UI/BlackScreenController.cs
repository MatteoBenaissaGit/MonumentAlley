using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BlackScreenController : MonoBehaviour
    {
        [SerializeField] private Image _blackScreen;
        
        private void Awake()
        {
            _blackScreen.color = new Color(0, 0, 0, 0);
        }

        public void Set(bool active, float time)
        {
            _blackScreen.DOKill();
            _blackScreen.DOFade(active ? 1 : 0, time);
        }
    }
}