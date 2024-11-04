using DefaultNamespace;
using UnityEngine;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TouchEffect _touchEffectPrefab;
        
        public void TouchEffectAt(Vector3 position, Vector3 up)
        {
            TouchEffect touchEffect = Instantiate(_touchEffectPrefab, transform);
            touchEffect.Play(position, up);
        }
    }
}
