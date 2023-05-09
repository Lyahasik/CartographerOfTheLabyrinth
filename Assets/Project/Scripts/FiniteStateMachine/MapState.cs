using UnityEngine;
using Zenject;

using Audio;
using UI;
using UI.Map;
using UI.Alerts;

namespace FiniteStateMachine
{
    public class MapState : GameState
    {
        private MapPanel _mapPanel;
        private TeleportPanel _teleportPanel;
        private MouseHandler _mouseHandler;

        [Inject]
        public void Construct(MapPanel mapPanel,
            TeleportPanel teleportPanel,
            MouseHandler mouseHandler)
        {
            _mapPanel = mapPanel;
            _teleportPanel = teleportPanel;
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            AudioHandler.DeactivateAll();
            base.Enter(gameMashine);
        
            _mouseHandler.ActivateCursor();
            _mapPanel.Activate(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.M))
            {
                _mapPanel.Deactivate();
            }
        }

        public override void Exit()
        {
            _mapPanel.Activate(false);
            _teleportPanel.DeactivateAllWindows();
        }
    }
}
