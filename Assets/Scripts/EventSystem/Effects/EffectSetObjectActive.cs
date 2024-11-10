using System.Collections;
using EventSystem.Effects;
using UnityEngine;
using Event = EventSystem.Event;

namespace Art.VFX.Scripts.Effects
{
    public class EffectSetObjectActive : Effect
    {
        public GameObject ObjectToSet;
        public bool SetActive;
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.96f, 0.99f);

        public override string ToString()
        {
            return $"Set {(ObjectToSet == null ? "___" : $"{ObjectToSet.name}")} {(SetActive ? "active" : "inactive")}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            ObjectToSet?.SetActive(SetActive);
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}