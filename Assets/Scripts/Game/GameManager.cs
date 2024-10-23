using MatteoBenaissaLibrary.SingletonClassBase;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Camera _camera;
        [SerializeField] private UIManager _ui;

        private InputsManager _inputs;

        protected override void Awake()
        {
            base.Awake();

            _inputs = new InputsManager();
            _inputs.Initialize();

            _inputs.OnTouch += OnTouch;
        }

        private void OnTouch(Vector2 position)
        {
            Ray ray = _camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                if (hit.transform.gameObject.TryGetComponent(out BlockController block))
                {
                    _ui.TouchEffectAt(block.WalkPoint);
                    _player.GoToBlock(block);
                }
            }
        }
    }
}
