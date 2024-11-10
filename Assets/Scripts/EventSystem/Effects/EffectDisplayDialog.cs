using System.Collections;
using Game;
using UI;
using UnityEngine;

namespace EventSystem.Effects
{
    public class EffectDisplayDialog : Effect
    {
        public string Dialog;
        public float Time;
        public override Color EffectColorInEditor { get; } = new Color(0.2f, 1f, 0.14f);

        public override string ToString()
        {
            return $"Set dialog";
        }
        
        public override IEnumerator Execute(Event gameEvent, GameObject gameObject)
        {
            DialogManager dialog = GameManager.Instance.UI.Dialog;
            dialog.ShowDialog(Dialog, Time);
            
            while (dialog.State != DialogState.Hidden)
            {
                yield return new WaitForSecondsRealtime(0.1f);
            }
        }
    }
}