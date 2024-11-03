using System;
using DG.Tweening;
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

        private float _currentRotationAmount;
        
        protected override void InternalPressed()
        {
        }

        protected override void InternalRelease()
        {
            float closest = Mathf.Round(_currentRotationAmount / 90) * 90;
            int step = (int)closest / 90;
         
            Quaternion rotation = Quaternion.Euler(new Vector3(_rotationAxis.x,_rotationAxis.y,_rotationAxis.z) * (step * 90));
            transform.DOKill();
            transform.DORotate(rotation.eulerAngles, 1f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                SetStep(step);
            });
        }

        protected override void ManageDirection(Vector2 direction)
        {
            if (direction.magnitude < 0.1f) return;
            
            float distance = Vector2.Distance(LastPressedPosition, LastPressedPosition + direction);

            bool biggestMovementIsX = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
            
            if (biggestMovementIsX)
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
            transform.rotation = Quaternion.Euler(new Vector3(_rotationAxis.x,_rotationAxis.y,_rotationAxis.z) * _currentRotationAmount);
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
    }
}