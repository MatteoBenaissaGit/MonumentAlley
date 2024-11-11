using System.Collections;
using Game;
using UI;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectInputsActive : Effect
    {
        public bool Active;
        public override Color EffectColorInEditor { get; } = new Color(0.48f, 0.88f, 0.65f);

        public override string ToString()
        {
            return $"Set inputs {(Active ? "active" : "inactive")}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            GameManager.Instance.InputsActive = Active;
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}