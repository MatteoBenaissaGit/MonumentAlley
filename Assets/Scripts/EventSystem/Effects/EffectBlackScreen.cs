using System.Collections;
using Game;
using UI;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectBlackScreen : Effect
    {
        public bool Active;
        public float Time;
        public override Color EffectColorInEditor { get; } = Color.black;

        public override string ToString()
        {
            return $"Set black screen {(Active ? "active" : "inactive")} in {Time}s";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            GameManager.Instance.UI.BlackScreen.Set(Active, Time);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}