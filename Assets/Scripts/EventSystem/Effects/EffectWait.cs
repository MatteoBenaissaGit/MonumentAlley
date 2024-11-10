using System.Collections;
using EventSystem.Effects;
using UnityEngine;
using Event = EventSystem.Event;

namespace Art.VFX.Scripts.Effects
{
    public class EffectWait : Effect
    {
        public float Duration;
        public override Color EffectColorInEditor { get; } = new Color(0.3f, 1f, 0.39f);

        public override string ToString()
        {
            return $"Wait for {Duration}s";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            yield return new WaitForSecondsRealtime(Duration);
        }
    }
}