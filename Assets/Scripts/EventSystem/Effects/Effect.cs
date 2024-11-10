using System;
using System.Collections;
using UnityEngine;
using Event = EventSystem.Event;

namespace EventSystem.Effects
{
    [Serializable]
    public class Effect
    {
        public bool Enabled = true;
        public virtual Color EffectColorInEditor { get; }
        public Event Event { get; set; }

        public override string ToString()
        {
            return string.Empty;
        }

        public virtual IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            yield break;
        }
    }
}