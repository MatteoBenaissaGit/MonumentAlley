using DefaultNamespace;
using UI;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [field:SerializeField] public DialogManager Dialog { get; private set; }
        [field: SerializeField] public BlackScreenController BlackScreen { get; private set; }
        
        [SerializeField] private TouchEffect _touchEffectPrefab;
        
        public void TouchEffectAt(Vector3 position, Vector3 up)
        {
            TouchEffect touchEffect = Instantiate(_touchEffectPrefab, transform);
            touchEffect.Play(position, up);
        }
    }
}
