using System.Collections;
using MatteoBenaissaLibrary.AudioManager;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectPlaySound : Effect
    {
        public SoundEnum Sound;
        public float Volume = 0.1f;
        
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.38f, 0.88f);

        public override string ToString()
        {
            return $"Play {Sound} sound";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (SoundManager.Instance == null) yield break;
            
            SoundManager.Instance.PlaySound(Sound, Volume);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}