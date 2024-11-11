using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectMoveObject : Effect
    {
        public GameObject ObjectToMove;
        public Vector3 Offset;
        public Ease Ease = Ease.Linear;
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
            
            if (Ease == Ease.Unset) Ease = Ease.Linear;
            
            ObjectToMove.transform.DOComplete();
            ObjectToMove.transform.DOLocalMove(ObjectToMove.transform.localPosition + Offset, Time).SetEase(Ease);
            
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}