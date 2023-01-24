using Zenject;

namespace CartographerOfTheLabyrinth.FiniteStateMachine
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
            if (gameState == null)
            {
                ResetState();
                return;
            }

            if (_currentState == gameState)
                return;
            
            _previousState = _currentState;
            _currentState = gameState;
            
            gameState.Enter(this);
        }

        private void ResetState()
        {
            _currentState = null;
            
            if (_previousState != null)
            {
                GameState newState = _previousState;
                _previousState = null;
                
                Enter(newState);
            }
        }

        public void HandleInput()
        {
            _currentState?.HandleInput();
        }
    }
}
