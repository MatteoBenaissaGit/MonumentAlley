using System.Collections;
using System.Linq;
using EventSystem.Effects;
using Game;
using UnityEngine;
using Event = EventSystem.Event;

namespace Art.VFX.Scripts.Effects
{
    public class EffectEnableBlock : Effect
    {
        public GameObject Block;
        public bool Enable = true;
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.96f, 0.99f);

        public override string ToString()
        {
            if (Block == null) return "Enable Block : no block references";
            
            return $"{(Enable ? "Enable" : "Disable")} {Block.name}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (Block == null) yield break;

            BlockController[] blocks = Block.GetComponentsInChildren<BlockController>();
            blocks.ToList().ForEach(x => x.Enabled = Enable);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}