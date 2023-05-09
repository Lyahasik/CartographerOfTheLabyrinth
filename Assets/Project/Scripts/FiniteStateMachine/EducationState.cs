using Zenject;

using Audio;
using UI;
using UI.Gameplay.Education;

namespace FiniteStateMachine
{
    public class EducationState : GameState
    {
        private EducationPanel _educationPanel;
        private MouseHandler _mouseHandler;

        [Inject]
        public void Construct(EducationPanel mapPanel, MouseHandler mouseHandler)
        {
            _educationPanel = mapPanel;
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            AudioHandler.DeactivateAll();
            base.Enter(gameMashine);
        
            _mouseHandler.ActivateCursor();
            _educationPanel.Activate(true);
        }

        public override void Exit()
        {
            _educationPanel.Activate(false);
        }
    }
}
