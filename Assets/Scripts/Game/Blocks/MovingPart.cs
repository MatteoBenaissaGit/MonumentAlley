using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Blocks
{
    /// <summary>
    /// This class is used to make select a block & a path to set active or inactive at specific move step <see cref="BlockController"/>
    /// </summary>
    [Serializable]
    public class BlockMoveStepPath
    {
        [field:SerializeField] public BlockController Block { get; private set; }
        [field:SerializeField] public BlockController ToBlock { get; private set; }
        [field: SerializeField] public List<int> ActiveSteps { get; private set; } = new();
    }
    
    public abstract class MovingPart : MonoBehaviour
    {
        [Header("Visual")] 
        [SerializeField] private List<Renderer> _glowingParts;

        [Header("Blocks")] 
        [SerializeField] protected List<BlockMoveStepPath> ActiveBlockSteps;
        [SerializeField] protected int NumberOfSteps;
        [SerializeField] protected int StartStep;

        public int CurrentStep { get; set; }
        public bool IsPressed { get; set; }
        protected float CurrentMovementStep { get; set; }
        protected bool CanBeMoved { get; set; } = true;
        protected Vector2 LastPressedPosition { get; set; }

        private static readonly int Glowing = Shader.PropertyToID("_Glowing");

        private void Start()
        {
            SetStep(StartStep);
            BlockController[] blocks = GetComponentsInChildren<BlockController>();
            foreach (BlockController block in blocks)
            {
                block.MovingPart = this;
            }
            
            InternalStart();
        }

        protected abstract void InternalStart();
        
        protected void Update()
        {
            HandleMovement();
        }

        public void GetPressed(Vector2 position)
        {
            if (IsPressed || CanBeMoved == false) return;
            
            IsPressed = true;
            LastPressedPosition = position;

            foreach (Renderer part in _glowingParts)
            {
                part.material.DOFloat(1, Glowing, 0.5f);
            }

            InternalPressed();
        }
        
        protected abstract void InternalPressed();

        public void GetReleased()
        {
            if (IsPressed == false) return;
            
            IsPressed = false;
            
            foreach (Renderer part in _glowingParts)
            {
                part.material.DOFloat(0, Glowing, 0.5f);
            }
            
            InternalRelease();
        }

        protected abstract void InternalRelease();

        private void HandleMovement()
        {
            if (IsPressed == false | GameManager.Instance.Inputs.IsPressingPosition(out Vector2 position) == false)
            {
                return;
            }
            
            Vector2 direction = position - LastPressedPosition;
            ManageDirection(direction);
            LastPressedPosition = position;
        }

        protected abstract void ManageDirection(Vector2 direction);

        protected virtual void SetStep(int step)
        {
            CurrentStep = step;
            
            foreach (BlockMoveStepPath blockMoveStepPath in ActiveBlockSteps)
            {
                bool isActive = blockMoveStepPath.ActiveSteps.Contains(step);
                blockMoveStepPath.Block.SetPathToActive(blockMoveStepPath.ToBlock, isActive);
                blockMoveStepPath.ToBlock.SetPathToActive(blockMoveStepPath.Block, isActive);
            }
        }

        protected void DisableAllPaths()
        {
            foreach (BlockMoveStepPath blockMoveStepPath in ActiveBlockSteps)
            {
                blockMoveStepPath.Block.SetPathToActive(blockMoveStepPath.ToBlock, false);
            }
        }

        public abstract void PlayerIsOnMovingPart(bool playerIsOn);
        
        protected void SetCanBeMoved(bool canBeMoved)
        {
            if (CanBeMoved == canBeMoved) return;
            
            CanBeMoved = canBeMoved;
            SetCanBeMovedInternal(canBeMoved);
        }

        protected abstract void SetCanBeMovedInternal(bool canBeMoved);
    }
}
