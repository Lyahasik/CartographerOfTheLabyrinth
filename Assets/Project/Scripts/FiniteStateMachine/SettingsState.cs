using UnityEngine;
using Zenject;

using UI.Settings;

namespace FiniteStateMachine
{
    public class SettingsState : GameState
    {
        private SettingsPanel _settingsPanel;

        [Inject]
        public void Construct(SettingsPanel settingsPanel)
        {
            _settingsPanel = settingsPanel;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _settingsPanel.Activate(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.T))
            {
                _settingsPanel.Deactivate();
            }
        }

        public override void Exit()
        {
            _settingsPanel.Activate(false);
        }
    }
}
