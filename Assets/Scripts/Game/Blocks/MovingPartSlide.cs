using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Blocks
{
    public enum SlideOrientation
    {
        LeftRight = 0,
        UpDown = 1
    }
    
    public class MovingPartSlide : MovingPart
    {
        [Header("Slider")]
        [SerializeField] private SlideOrientation _slideOrientation;
        [SerializeField] private float _movementMultiplier = 1;

        private float _currentPosition;
        private float _baseAxisPosition;
        private Vector3 _basePosition;

        private void Awake()
        {
            _basePosition = transform.position;
            _baseAxisPosition = _slideOrientation switch
            {
                SlideOrientation.LeftRight => transform.position.x,
                SlideOrientation.UpDown => transform.position.y,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void InternalStart()
        {
            
        }

        protected override void InternalPressed()
        {
            _slideTween?.Kill();
            DisableAllPaths();
            CurrentStep = -1;
        }

        private Tween _slideTween;
        protected override void InternalRelease()
        {
            float closest = Mathf.Round(_currentPosition);
            int step = (int)closest;

            Vector3 position = _slideOrientation switch
            {
                SlideOrientation.LeftRight => new Vector3(_baseAxisPosition + step, _basePosition.y, _basePosition.z),
                SlideOrientation.UpDown => new Vector3(_basePosition.x, _baseAxisPosition + step, _basePosition.z),
                _ => throw new ArgumentOutOfRangeException()
            };
            transform.DOKill();
            _slideTween = transform.DOMove(position, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
            {
                SetStep(step);
            });
        }

        protected override void SetStep(int step)
        {
            base.SetStep(step);
            
            SetStepPosition(step);
        }

        private void SetStepPosition(int step)
        {
            _currentPosition = step;
            transform.position = _slideOrientation switch
            {
                SlideOrientation.LeftRight => new Vector3(_baseAxisPosition + step, _basePosition.y, _basePosition.z),
                SlideOrientation.UpDown => new Vector3(_basePosition.x, _baseAxisPosition + step, _basePosition.z),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        protected override void ManageDirection(Vector2 direction)
        {
            if (_slideOrientation == SlideOrientation.UpDown)
            {
                Slide(transform.up, direction.y);
            }
            else
            {
                Slide(transform.right, direction.x);
            }
        }

        public override void PlayerIsOnMovingPart(bool playerIsOn)
        {
        }

        protected override void SetCanBeMovedInternal(bool canBeMoved)
        {
        }

        public override void SetStepWithEase(int step, float time)
        {
            SetStep(step);
        }

        private void Slide(Vector3 axis, float amount)
        {
            amount *= _movementMultiplier;
            float movementAmount = amount * (_slideOrientation == SlideOrientation.UpDown ? Math.Sign(axis.y) : Math.Sign(axis.x));

            if (_currentPosition + movementAmount <= 0)
            {
                SetStepPosition(0);
                return;
            }
            if (_currentPosition + movementAmount >= NumberOfSteps)
            {
                SetStepPosition(NumberOfSteps);
                return;
            }
            
            transform.position += axis * amount;
            CurrentMovementStep += movementAmount;
            _currentPosition += movementAmount;
        }
    }
}