using System.Collections;
using Game.Blocks;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectSetMovingPartStep : Effect
    {
        public MovingPart MovingPart;
        public int Step;
        public float Time;
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.58f, 0.92f);

        public override string ToString()
        {
            return $"Set {(MovingPart == null ? "___" : $"{MovingPart.name}")} step {Step}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            MovingPart.SetStepWithEase(Step, Time);
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}