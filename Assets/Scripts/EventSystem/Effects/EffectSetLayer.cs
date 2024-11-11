using System.Collections;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectSetLayer : Effect
    {
        public GameObject ObjectToSet;
        public int Layer;
        public override Color EffectColorInEditor { get; } = new Color(0.42f, 0.4f, 0.42f);

        public override string ToString()
        {
            return $"Set {(ObjectToSet == null ? "___" : $"{ObjectToSet.name}")} layer to {Layer}";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            SetLayerRecursive(ObjectToSet, Layer);
            
            yield return new WaitForSecondsRealtime(0f);
        }

        private static void SetLayerRecursive(GameObject gameObject, int layer)
        {
            if (gameObject.TryGetComponent<PlayerController>(out _)) return;
            
            gameObject.layer = layer;
            foreach (Transform child in gameObject.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }
}