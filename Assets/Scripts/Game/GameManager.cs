using System;
using System.Collections.Generic;
using Game.Blocks;
using MatteoBenaissaLibrary.SingletonClassBase;
using UI;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public InputsManager Inputs => _inputs;
        public PlayerController Player => _player;
        public UIManager UI => _ui;
        public BlockController EndBlock => _endBlock;
        public bool InputsActive { get; set; } = true;
        public List<BlockToBlock> BlockedPath { get; private set; } = new List<BlockToBlock>();

        [Header("Game")]
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private UIManager _ui;
        [SerializeField] private BlockController _endBlock;

        [Header("Events")]
        [SerializeField] private EventSystem.Event _startEvent;
        [SerializeField] private EventSystem.Event _endEvent;

        private InputsManager _inputs;
        private MovingPart _currentMovingPartPressed;

        private void Start()
        {
            if (_startEvent != null)
            {
                _startEvent.OnEnd += SetInputs;
                _startEvent.Launch();
                return;
            }
            SetInputs();
        }

        private void SetInputs()
        {
            _inputs = new InputsManager();
            _inputs.Initialize();

            _inputs.OnTouch += OnTouch;
            _inputs.OnRelease += OnRelease;
        }

        private void OnTouch(Vector2 position)
        {
            if (InputsActive == false)
            {
                return;
            }
            
            Ray ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                GameObject hitGameObject = hit.transform.gameObject;
                if (hitGameObject.TryGetComponent(out MovingPart movingPart))
                {
                    movingPart.GetPressed(position);
                    _currentMovingPartPressed = movingPart;
                }
                else if (hitGameObject.TryGetComponent(out BlockController block))
                {
                    Vector3 walkPoint = block.WalkPoint;
                    if (block.Type == BlockType.Ladder)
                    {
                        if (block.LadderTop == false)
                        {
                            return;
                        }   
                        walkPoint = block.LadderTopWalkPoint;
                    }
                    
                    _ui.TouchEffectAt(walkPoint, block.Type == BlockType.Stair ? (walkPoint - block.transform.position).normalized : block.transform.up);
                    _player.GoToBlock(block);
                }
            }
        }
        
        private void OnRelease(Vector2 position)
        {
            if (InputsActive == false)
            {
                return;
            }
            
            _currentMovingPartPressed?.GetReleased();
            _currentMovingPartPressed = null;
        }

        public void EndLevel()
        {
            InputsActive = false;
            _endEvent.Launch();
            _endEvent.OnEnd += NextLevel;
        }

        private void NextLevel()
        {
            LevelManager.Instance.NextLevel();
        }
    }

    [Serializable]
    public class BlockToBlock
    {
        public BlockToBlock(BlockController startBlock, BlockController endBlock)
        {
            StartBlock = startBlock;
            EndBlock = endBlock;
        }

        [field:SerializeField] public BlockController StartBlock { get; private set; }
        [field:SerializeField] public BlockController EndBlock { get; private set; }
    }
}
