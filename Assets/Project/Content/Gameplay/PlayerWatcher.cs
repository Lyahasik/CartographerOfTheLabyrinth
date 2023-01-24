using CartographerOfTheLabyrinth.Gameplay.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerWatcher : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private CinemachineVirtualCamera _cinemachine;

    [Inject]
    public void Construct(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    
    public void Start()
    {
        _cinemachine = GetComponent<CinemachineVirtualCamera>();

        _cinemachine.Follow = _playerMovement.transform;
        _cinemachine.LookAt = _playerMovement.transform;
    }

    public void ResetPosition()
    {
        _cinemachine.ForceCameraPosition(_playerMovement.transform.position, _playerMovement.transform.rotation);
    }
}
