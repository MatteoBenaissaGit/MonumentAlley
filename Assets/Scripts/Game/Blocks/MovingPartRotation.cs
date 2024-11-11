using System;
using DG.Tweening;
using MatteoBenaissaLibrary.AudioManager;
using UnityEngine;

namespace Game.Blocks
{
    public enum Rotation
    {
        Clockwise,
        AntiClockwise
    }
    
    public class MovingPartRotation : MovingPart
    {
        [Header("Rotation")]
        [SerializeField] private Vector3Int _rotationAxis;
        [SerializeField] private bool _inverseHorizontal;
        [SerializeField] private bool _inverseVertical;
        [SerializeField] private float _rotationSpeedMultiplier = 1;

        [Header("Visual")] 
        [SerializeField] private GameObject _handle;
        [SerializeField] private bool _makeGlowAtStart;

        private Vector3 _baseHandleLocalScale;
        private Vector3 _baseLocalRotation;
        private float _currentRotationAmount;
        private bool _doStartGlow;

        private void Awake()
        {
            _baseHandleLocalScale = _handle.transform.localScale;
            _baseLocalRotation = transform.localRotation.eulerAngles;
        }

        protected override void InternalStart()
        {
            _doStartGlow = _makeGlowAtStart;
        }

        protected override void InternalPressed()
        {
            _doStartGlow = false;
            
            _rotationTween?.Kill();
            DisableAllPaths();
            CurrentStep = -1;
            
            SoundManager.Instance?.PlaySound(SoundEnum.unlock, 0.03f);
        }

        private float _time;
        protected override void InternalUpdate()
        {
            if (_doStartGlow == false) return;
            
            _time += Time.deltaTime * 4;
            float glow = Mathf.Sin(_time) * 0.5f + 0.5f;
            foreach (Renderer part in GlowingParts)
            {
                part.material.SetFloat(Glowing, glow);
            }
        }

        private Tween _rotationTween;
        protected override void InternalRelease()
        {
            float closest = Mathf.Round(_currentRotationAmount / 90) * 90;
            int step = (int)closest / 90;
            if (step == 4) step = 0;
         
            Quaternion rotation = Quaternion.Euler(new Vector3(_rotationAxis.x,_rotationAxis.y,_rotationAxis.z) * (step * 90) + _baseLocalRotation);
            transform.DOKill();
            _rotationTween = transform.DORotate(rotation.eulerAngles, 0.75f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                SetStep(step);
            });
            
            SoundManager.Instance?.PlaySound(SoundEnum.unlock, 0.025f);
        }

        protected override void ManageDirection(Vector2 direction)
        {
            if (direction.magnitude < 0.1f) return;
            
            float distance = Vector2.Distance(LastPressedPosition, LastPressedPosition + direction);

            bool biggestMovementIsX = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
            
            if (false)//(biggestMovementIsX && Mathf.Abs(direction.y) <= 0.5f)
            {
                if (_inverseHorizontal)
                {
                    direction.x *= -1;
                }
                Rotate(direction.x > 0 ? Rotation.Clockwise : Rotation.AntiClockwise, distance);
            }
            else
            {
                if (_inverseVertical)
                {
                    direction.y *= -1;
                }
                Rotate(direction.y > 0 ? Rotation.Clockwise : Rotation.AntiClockwise, distance);
            }
        }

        protected override void SetStep(int step)
        {
            base.SetStep(step);

            _currentRotationAmount = step * 90;
            transform.rotation = Quaternion.Euler(new Vector3(_rotationAxis.x,_rotationAxis.y,_rotationAxis.z) * _currentRotationAmount + _baseLocalRotation);
        }

        public override void PlayerIsOnMovingPart(bool playerIsOn)
        {                
            SetCanBeMoved(playerIsOn == false);
        }

        protected override void SetCanBeMovedInternal(bool canBeMoved)
        {
            SetHandle(canBeMoved);
        }

        private void Rotate(Rotation rotation, float amount)
        {
            amount *= _rotationSpeedMultiplier;
            
            float rotationAmount = amount * (rotation == Rotation.Clockwise ? 1 : -1);
            CurrentMovementStep += rotationAmount;
            
            transform.Rotate(_rotationAxis, rotationAmount);
            
            _currentRotationAmount += rotationAmount;
            if (_currentRotationAmount > 360)
            {
                _currentRotationAmount -= 360;
            }
            else if (_currentRotationAmount < 0)
            {
                _currentRotationAmount += 360;
            }
        }
        
        private void SetHandle(bool active)
        {
            SoundManager.Instance?.PlaySound(SoundEnum.slidePartClose, 0.025f);

            _handle.transform.DOKill();
            _handle.transform.DOScale(active ? _baseHandleLocalScale : Vector3.zero, 0.4f).SetEase(Ease.InBack);
        }
    }
}