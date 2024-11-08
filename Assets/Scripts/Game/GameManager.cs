using Game.Blocks;
using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public InputsManager Inputs => _inputs;
        
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private UIManager _ui;

        private InputsManager _inputs;
        private MovingPart _currentMovingPartPressed;

        protected override void Awake()
        {
            base.Awake();

            _inputs = new InputsManager();
            _inputs.Initialize();

            _inputs.OnTouch += OnTouch;
            _inputs.OnRelease += OnRelease;
        }

        private void OnTouch(Vector2 position)
        {
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
                    
                    Vector3 up = (walkPoint - block.transform.position).normalized;
                    _ui.TouchEffectAt(walkPoint, up);
                    _player.GoToBlock(block);
                }
            }
        }
        
        private void OnRelease(Vector2 position)
        {
            _currentMovingPartPressed?.GetReleased();
            _currentMovingPartPressed = null;
        }
    }
}
