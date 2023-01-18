using CartographerOfTheLabyrinth.Gameplay.Player;
using Cinemachine;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class PlayerWatcher : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    [Inject]
    public void Construct(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }
    
    public void Start()
    {
        var cinemachine = GetComponent<CinemachineVirtualCamera>();

        cinemachine.Follow = _playerMovement.transform;
        cinemachine.LookAt = _playerMovement.transform;
    }
}
