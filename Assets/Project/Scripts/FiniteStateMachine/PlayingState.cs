using UnityEngine;
using Zenject;

using Gameplay.Player;
using UI.Alerts;

namespace FiniteStateMachine
{
    public class PlayingState : GameState
    {
        private DiContainer _container;
        private TeleportPanel _teleportPanel;
        private PlayerMovement _playerMovement;

        [Inject]
        public void Construct(DiContainer container, TeleportPanel teleportPanel, PlayerMovement playerMovement)
        {
            _container = container;
            _teleportPanel = teleportPanel;
            _playerMovement = playerMovement;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _playerMovement.IsFreeze = false;
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _gameMashine.Enter(_container.Instantiate<MapState>());
            }
        }

        public override void Exit()
        {
            _playerMovement.IsFreeze = true;
            _teleportPanel.DeactivateAllWindows();
        }
    }
}
