using System;
using System.Collections;
using System.Collections.Generic;
using EventSystem.Effects;
using UnityEngine;

namespace EventSystem
{
    [Serializable]
    public class Event : MonoBehaviour
    {
        [SerializeReference] public List<Effect> Effects = new List<Effect>();

        public Action OnEnd { get; set; }
        
        private void OnEnable()
        {
            Effects.Remove(null);
            
            Effects.ForEach(x => x.Event = this);
        }

        public void Launch()
        {
            StartCoroutine(Execute(gameObject));
        }

        public IEnumerator Execute(GameObject gameObjectToExecute)
        {
            foreach (Effect effect in Effects)
            {
                if (effect.Enabled == false) continue;
                yield return effect.Execute(this, gameObjectToExecute);
            }
            OnEnd?.Invoke();
        }
    }
}