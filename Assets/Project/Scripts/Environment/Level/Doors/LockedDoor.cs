using UnityEngine;
using Zenject;

using Gameplay.Player;
using Environment.Level;
using Gameplay.Items;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class LockedDoor : MonoBehaviour
{
    private readonly int _openingId = Animator.StringToHash("Opening");

    private PlayerInventory _playerInventory;
    
    private Animator _animator;

    private Level _level;

    [Inject]
    public void Construct(PlayerInventory playerInventory)
    {
        _playerInventory = playerInventory;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(Level level)
    {
        _level = level;
    }

    private void DestroyYourself()
    {
        _level.DestroyObject(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            if (_playerInventory.ContainsItem(ItemType.DoorKey))
            {
                _animator.SetTrigger(_openingId);
                _playerInventory.UseItem(ItemType.DoorKey);
            }
        }
    }
}
