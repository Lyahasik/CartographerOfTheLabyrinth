using Environment.Level;
using UnityEngine;

using Gameplay.Player;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Animator))]
public class LockedDoor : MonoBehaviour
{
    private readonly int _openingId = Animator.StringToHash("Opening");
    
    private Animator _animator;

    private Level _level; 

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
            _animator.SetTrigger(_openingId);
        }
    }
}
