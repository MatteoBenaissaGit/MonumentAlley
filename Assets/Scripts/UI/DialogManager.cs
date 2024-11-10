using System;
using Febucci.UI;
using UnityEngine;

namespace UI
{
    public enum DialogState
    {
        Hidden,
        Writing,
        Written
    }
    
    public class DialogManager : MonoBehaviour
    {
        [SerializeField] private TextAnimator _textAnimator;
        [SerializeField] private TextAnimatorPlayer _textAnimatorPlayer;
        [SerializeField] private GameObject _dialogPanel;
        
        public DialogState State { get; private set; }
        
        private float _currentDialogTime;

        private void Awake()
        {
            _dialogPanel.SetActive(false);
        }

        private void Update()
        {
            if (State == DialogState.Writing)
            {
                _currentDialogTime -= Time.deltaTime;
                if (_currentDialogTime <= 0)
                {
                    ClosePanel();
                }
            }
        }

        public void ShowDialog(string dialog, float time)
        {
            _dialogPanel.SetActive(true);
            
            _textAnimator.SetText("",false);
            _textAnimatorPlayer.ShowText(dialog);
            
            State = DialogState.Writing;

            _currentDialogTime = time;
        }

        public void EndWriting()
        {
            if (State != DialogState.Writing)
            {
                return;
            }
            
            State = DialogState.Written;
        }
        
        public void ClosePanel()
        {
            State = DialogState.Hidden;
            _dialogPanel.SetActive(false);
        }
        
        public void OnSkip()
        {
            switch (State)
            {
                case DialogState.Hidden:
                    break;
                case DialogState.Writing:
                    EndWriting();
                    break;
                case DialogState.Written:
                    ClosePanel();
                    break;
            }
        }
    }
}