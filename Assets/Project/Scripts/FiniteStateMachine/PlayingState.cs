using UnityEngine;
using Zenject;

using Gameplay.Player;
using UI.Alerts;
using UI.Icons;

namespace FiniteStateMachine
{
    public class PlayingState : GameState
    {
        private DiContainer _container;
        private TeleportPanel _teleportPanel;
        private IconsPanel _iconsPanel;
        private PlayerMovement _playerMovement;

        [Inject]
        public void Construct(DiContainer container,
            TeleportPanel teleportPanel,
            IconsPanel iconsPanel,
            PlayerMovement playerMovement)
        {
            _container = container;
            _teleportPanel = teleportPanel;
            _iconsPanel = iconsPanel;
            _playerMovement = playerMovement;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _playerMovement.IsFreeze = false;
            _iconsPanel.gameObject.SetActive(true);
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
            _iconsPanel.gameObject.SetActive(false);
            _teleportPanel.DeactivateAllWindows();
        }
    }
}
