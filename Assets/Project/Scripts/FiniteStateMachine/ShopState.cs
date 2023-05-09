using UnityEngine;
using Zenject;

using Audio;
using UI;
using UI.Shop;

namespace FiniteStateMachine
{
    public class ShopState : GameState
    {
        private ShopPanel _shopPanel;
        private MouseHandler _mouseHandler;

        [Inject]
        public void Construct(ShopPanel shopPanel, MouseHandler mouseHandler)
        {
            _shopPanel = shopPanel;
            _mouseHandler = mouseHandler;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            AudioHandler.DeactivateAll();
            base.Enter(gameMashine);
        
            _mouseHandler.ActivateCursor();
            _shopPanel.Activate(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.G))
            {
                _shopPanel.Deactivate();
            }
        }

        public override void Exit()
        {
            _shopPanel.Activate(false);
        }
    }
}
