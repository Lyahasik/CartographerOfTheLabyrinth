using Gameplay.Player;
using UnityEngine;
using Zenject;

public class BuffsHandler : ITickable
{
    private PlayerMovement _playerMovement;

    [Inject]
    public void Construct(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    public void Tick()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            TryActivateSpeedBoost();
    }

    public bool TryActivateSpeedBoost()
    {
        return _playerMovement.TryActivateBoost();
    }
}
