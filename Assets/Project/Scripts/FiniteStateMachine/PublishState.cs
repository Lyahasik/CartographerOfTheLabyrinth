using UnityEngine;
using Zenject;

using Audio;
using UI;

namespace FiniteStateMachine
{
    public class PublishState : GameState
    {
        private const string _musicClipName = "Music";
        
        private MouseHandler _mouseHandler;

        private bool _isChangedCursor;

        [Inject]
        public void Construct(MouseHandler mouseHandler)
        {
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
            
            // if (!_mouseHandler.IsActive)
            // {
            //     _isChangedCursor = true;
            _mouseHandler.ActivateCursor();
            // // }
            //
            AudioHandler.DeactivateAll();
            // Time.timeScale = 0f;
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.G))
            {
                _gameMashine.ResetState();
            }
        }

        public override void Exit()
        {
            // if (_isChangedCursor)
            // {
            //     _isChangedCursor = false;
                _mouseHandler.DeactivateCursor();
            // }
            
            // Time.timeScale = 1f;
            AudioHandler.ActivateClip(_musicClipName);
        }
    }
}
