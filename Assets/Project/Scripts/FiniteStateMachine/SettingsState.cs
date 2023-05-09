using UnityEngine;
using Zenject;

using Audio;
using UI;
using UI.Settings;

namespace FiniteStateMachine
{
    public class SettingsState : GameState
    {
        private SettingsPanel _settingsPanel;
        private MouseHandler _mouseHandler;

        [Inject]
        public void Construct(SettingsPanel settingsPanel, MouseHandler mouseHandler)
        {
            _settingsPanel = settingsPanel;
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            AudioHandler.DeactivateAll();
            base.Enter(gameMashine);
        
            _mouseHandler.ActivateCursor();
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
