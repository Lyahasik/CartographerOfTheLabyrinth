using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.UI.Map
{
    public class MapController : ITickable
    {
        private MapPanel _mapPanel;

        [Inject]
        public void Construct(MapPanel mapPanel)
        {
            _mapPanel = mapPanel;
        }

        public void Tick()
        {
            TrySwitchStateMap();
        }

        private void TrySwitchStateMap()
        {
            if (!Input.GetKeyDown(KeyCode.M))
                return;
        
            _mapPanel.SwitchActive();
        }
    }
}
