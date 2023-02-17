using UnityEngine;
using Zenject;

using UI.Shop;

namespace FiniteStateMachine
{
    public class ShopState : GameState
    {
        private ShopPanel _shopPanel;

        [Inject]
        public void Construct(ShopPanel shopPanel)
        {
            _shopPanel = shopPanel;
        }
    
        public override void Enter(GameMashine gameMashine)
        {
            base.Enter(gameMashine);
        
            _shopPanel.Activate(true);
        }

        public override void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Escape)
                || Input.GetKeyDown(KeyCode.G))
            {
                _gameMashine.ResetState();
            }
        }

        public override void Exit()
        {
            _shopPanel.Activate(false);
        }
    }
}
