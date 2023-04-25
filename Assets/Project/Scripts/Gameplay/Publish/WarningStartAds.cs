using TMPro;
using UnityEngine;

public class WarningStartAds : MonoBehaviour
{
    private const float _delay = 1f;

    [SerializeField] private GameObject _buttonContinue;
    [SerializeField] private GameObject _text;
    [SerializeField] private TMP_Text _textTime;
    [SerializeField] private int _amountTime;

    private int _timeLeft;
    private float _nextTimeUpdate;

    private void Start()
    {
        _buttonContinue.SetActive(false);
        _text.SetActive(true);
        
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
            return;
            
        if (_nextTimeUpdate > Time.time)
            return;
        
        _textTime.text = _timeLeft.ToString();

        _timeLeft--;
        if (_timeLeft < 0)
        {
            _text.SetActive(false);
            _buttonContinue.SetActive(true);
        }
        
        _nextTimeUpdate = Time.time + _delay;
    }
}
