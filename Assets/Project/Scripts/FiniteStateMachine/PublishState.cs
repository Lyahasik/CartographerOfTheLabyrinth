using Zenject;
using UnityEngine;

using UI;

namespace FiniteStateMachine
{
    public class PublishState : GameState
    {
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
            
            _mouseHandler.ActivateCursor();

            AudioListener.volume = 0f;
        }

        public override void Exit()
        {
            _mouseHandler.DeactivateCursor();
            
            AudioListener.volume = 1f;
        }
    }
}
