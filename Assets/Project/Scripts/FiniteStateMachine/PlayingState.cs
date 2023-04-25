using UnityEngine;
using Zenject;

using Gameplay.Player;
using Gameplay.Buffs;
using UI;
using UI.Alerts;
using UI.Gameplay;

namespace FiniteStateMachine
{
    public class PlayingState : GameState
    {
        private DiContainer _container;
        private TeleportPanel _teleportPanel;
        private GameplayPanel _gameplayPanel;
        private PlayerMovement _playerMovement;
        private BuffsHandler _buffsHandler;
        private MouseHandler _mouseHandler;

        [Inject]
        public void Construct(DiContainer container,
            TeleportPanel teleportPanel,
            GameplayPanel gameplayPanel,
            PlayerMovement playerMovement,
            BuffsHandler buffsHandler,
            MouseHandler mouseHandler)
        {
            _container = container;
            _teleportPanel = teleportPanel;
            _gameplayPanel = gameplayPanel;
            _playerMovement = playerMovement;
            _buffsHandler = buffsHandler;
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _mouseHandler.DeactivateCursor();
            _playerMovement.IsFreeze = false;
            _buffsHandler.IsFreeze = false;
            
            _gameplayPanel.gameObject.SetActive(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                _gameMashine.Enter(_container.Instantiate<MapState>());
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                _gameMashine.Enter(_container.Instantiate<SettingsState>());
            }
            
            if (Input.GetKeyDown(KeyCode.G))
            {
                _gameMashine.Enter(_container.Instantiate<ShopState>());
            }
        }

        public override void Exit()
        {
            _playerMovement.IsFreeze = true;
            _buffsHandler.IsFreeze = true;
            
            _teleportPanel.DeactivateAllWindows();
        }
    }
}
