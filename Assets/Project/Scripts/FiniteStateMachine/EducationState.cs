using Zenject;

using UI.Gameplay.Education;

namespace FiniteStateMachine
{
    public class EducationState : GameState
    {
        private EducationPanel _educationPanel;

        [Inject]
        public void Construct(EducationPanel mapPanel)
        {
            _educationPanel = mapPanel;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _educationPanel.Activate(true);
        }

        public override void Exit()
        {
            _educationPanel.Activate(false);
        }
    }
}
