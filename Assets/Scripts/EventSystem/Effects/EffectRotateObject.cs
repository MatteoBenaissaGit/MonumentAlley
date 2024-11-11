using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectRotateObject : Effect
    {
        public GameObject ObjectToRotate;
        public Vector3 Rotation;
        public float Time;
        
        public override Color EffectColorInEditor { get; } = new Color(0.97f, 0.29f, 1f);

        public override string ToString()
        {
            return $"Rotate {(ObjectToRotate == null ? "___" : $"{ObjectToRotate.name}")} to {Rotation}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            ObjectToRotate.transform.DOComplete();
            ObjectToRotate.transform.DORotate(Rotation, Time);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}