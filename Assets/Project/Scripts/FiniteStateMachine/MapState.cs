using CartographerOfTheLabyrinth.FiniteStateMachine;
using CartographerOfTheLabyrinth.UI.Map;
using UnityEngine;
using Zenject;

public class MapState : GameState
{
    private MapPanel _mapPanel;

    [Inject]
    public void Construct(MapPanel mapPanel)
    {
        _mapPanel = mapPanel;
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
            Exit();
        }
    }

    public override void Exit()
    {
        _mapPanel.Activate(false);
        
        _gameMashine.Enter(null);
    }
}
