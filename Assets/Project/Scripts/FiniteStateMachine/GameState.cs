using CartographerOfTheLabyrinth.FiniteStateMachine;

public abstract class GameState
{
    protected GameMashine _gameMashine;
    
    public virtual void Enter(GameMashine gameMashine)
    {
        _gameMashine = gameMashine;
    }
    
    public virtual void HandleInput() {}

    public virtual void Exit() {}
}
