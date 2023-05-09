using TMPro;
using UnityEngine;

public class WarningStartAds : MonoBehaviour
{
    private const float _delay = 1f;

    [SerializeField] private TMP_Text _textTime;
    [SerializeField] private int _amountTime;

    private int _timeLeft;
    private float _nextTimeUpdate;

    private void OnEnable()
    {
        _timeLeft = _amountTime;
        UpdateTime();
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if (_timeLeft < 0)
        {
            gameObject.SetActive(false);
            return;
        }
            
        if (_nextTimeUpdate > Time.time)
            return;
        
        _textTime.text = _timeLeft.ToString();

        _timeLeft--;
        
        _nextTimeUpdate = Time.time + _delay;
    }
}
