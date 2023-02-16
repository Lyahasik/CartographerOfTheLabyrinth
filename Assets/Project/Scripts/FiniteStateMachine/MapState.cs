using UnityEngine;
using Zenject;

using UI.Map;
using UI.Alerts;

namespace FiniteStateMachine
{
    public class MapState : GameState
    {
        private MapPanel _mapPanel;
        private TeleportPanel _teleportPanel;

        [Inject]
        public void Construct(MapPanel mapPanel, TeleportPanel teleportPanel)
        {
            _mapPanel = mapPanel;
            _teleportPanel = teleportPanel;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _mapPanel.Activate(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.M))
            {
                _gameMashine.ResetState();
            }
        }

        public override void Exit()
        {
            _mapPanel.Activate(false);
            _teleportPanel.DeactivateAllWindows();
        }
    }
}
