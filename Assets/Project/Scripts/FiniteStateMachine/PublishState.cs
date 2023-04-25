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
            
            _mouseHandler.ActivateCursor();
            
            AudioHandler.DeactivateAll();
        }

        public override void Exit()
        {
            _mouseHandler.DeactivateCursor();
            
            AudioHandler.ActivateClip(_musicClipName);
        }
    }
}
