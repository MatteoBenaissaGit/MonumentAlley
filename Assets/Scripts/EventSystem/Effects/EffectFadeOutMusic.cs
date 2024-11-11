using System.Collections;
using MatteoBenaissaLibrary.AudioManager;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectFadeOutMusic : Effect
    {
        public float Time = 1f;
        
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.38f, 0.88f);

        public override string ToString()
        {
            return $"Fade out music in {Time}s";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (SoundManager.Instance == null) yield break;
            
            SoundManager.Instance.FadeOutMusic(Time);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}