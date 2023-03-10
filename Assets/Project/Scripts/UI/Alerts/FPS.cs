using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.Alerts
{
    [RequireComponent(typeof(TMP_Text))]
    public class FPS : MonoBehaviour
    {
        private TMP_Text _text;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            StartCoroutine(UpdateFPS());
        }

        private IEnumerator UpdateFPS()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
            
                int fps = (int) (1f / Time.deltaTime);
        
                _text.text = $"FPS {fps}";
            }
        }
    }
}
