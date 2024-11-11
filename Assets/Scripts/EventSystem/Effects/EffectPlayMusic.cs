using System.Collections;
using MatteoBenaissaLibrary.AudioManager;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectPlayMusic : Effect
    {
        public AudioClip Music;
        public float Volume = 0.1f;
        
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.38f, 0.88f);

        public override string ToString()
        {
            return $"Play {Music} music";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (SoundManager.Instance == null) yield break;
            
            SoundManager.Instance.PlayMusic(Music, Volume);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}