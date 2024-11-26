﻿using System;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private LevelManager _levelManager;
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Image _blackScreen;
        [SerializeField] private Transform _paperFeedback;

        private void Start()
        {
            _playButton.onClick.AddListener(StartLevel);
            _quitButton.onClick.AddListener(Application.Quit);

            _blackScreen.DOFade(0, 1f);
        }

        private void StartLevel()
        {
            _paperFeedback.DOMove(_paperFeedback.position + _paperFeedback.forward * 3, 1f);
            
            _quitButton.GetComponent<MenuButton>().SetIsClicked();
            _blackScreen.DOFade(1, 1f).OnComplete(_levelManager.NextLevel);
        }
    }
}