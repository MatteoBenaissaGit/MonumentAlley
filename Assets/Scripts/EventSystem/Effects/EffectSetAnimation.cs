using System.Collections;
using Game;
using UnityEngine;

namespace EventSystem.Effects
{
    public enum AnimationType
    {
        Bool,
        Trigger
    }
    
    public class EffectSetAnimation : Effect
    {
        public string Name;
        public Animator Animator;
        public AnimationType Type;
        public bool SetActive;
        public override Color EffectColorInEditor { get; } = new Color(1f, 0.69f, 0.13f);

        public override string ToString()
        {
            return $"Set {Name} {Type} animation {(SetActive ? "active" : "inactive")}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            switch (Type)
            {
                case AnimationType.Bool:
                    Animator.SetBool(Name, SetActive);
                    break;
                case AnimationType.Trigger:
                    Animator.SetTrigger(Name);
                    break;
            }
            yield return new WaitForSecondsRealtime(0f);
        }
    }
}