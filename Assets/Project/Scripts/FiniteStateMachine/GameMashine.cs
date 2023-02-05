using Zenject;

namespace FiniteStateMachine
{
    public class GameMashine : IInitializable, ITickable
    {
        private DiContainer _container;
        
        private GameState _currentState;
        private GameState _previousState;

        [Inject]
        public void Construct(DiContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            Enter(_container.Instantiate<PlayingState>());
        }

        public void Tick()
        {
            HandleInput();
        }

        public void Enter(GameState gameState)
        {
            if (_currentState == gameState)
                return;
            
            _currentState?.Exit();
            _previousState = _currentState;
            
            _currentState = gameState;
            _currentState.Enter(this);
        }

        public void ResetState()
        {
            _currentState.Exit();
            _currentState = null;
            
            if (_previousState != null)
                Enter(_previousState);
        }

        public void HandleInput()
        {
            _currentState?.HandleInput();
        }
    }
}
