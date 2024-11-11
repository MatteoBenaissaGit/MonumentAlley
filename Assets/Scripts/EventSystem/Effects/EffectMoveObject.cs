using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectMoveObject : Effect
    {
        public GameObject ObjectToMove;
        public Vector3 Offset;
        public float Time;
        
        public override Color EffectColorInEditor { get; } = new Color(0.97f, 0.29f, 1f);

        public override string ToString()
        {
            return $"Move {(ObjectToMove == null ? "___" : $"{ObjectToMove.name}")} to {Offset}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            if (ObjectToMove == null)
            {
                yield break;
            }
            
            ObjectToMove.transform.DOComplete();
            ObjectToMove.transform.DOLocalMove(ObjectToMove.transform.localPosition + Offset, Time);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}