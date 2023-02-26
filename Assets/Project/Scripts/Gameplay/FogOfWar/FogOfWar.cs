using System;
using UnityEngine;

namespace Gameplay.FogOfWar
{
    public class FogOfWar : MonoBehaviour
    {
        [SerializeField] private RenderTexture _renderTexture;

        private Texture2D _progressFogTexture;
        private Rect _rectTexture;

        private float _delayUpdate = 3f;
        private float _nextUpdateTime;

        private void Start()
        {
            _progressFogTexture = new Texture2D(_renderTexture.width, _renderTexture.height);
            _rectTexture = new Rect(0, 0, _renderTexture.width, _renderTexture.height);
        
            LoadFog();
        }

        private void Update()
        {
            UpdateFog();
        }

        private void LoadFog()
        {
            Texture2D texture = new Texture2D(_renderTexture.width, _renderTexture.height);
        
            string stringFog = PlayerPrefs.GetString("Fog");

            if (stringFog == string.Empty)
                return;
        
            byte[] bytes = Convert.FromBase64String(stringFog);

            texture.LoadImage(bytes);
        
            Graphics.Blit(texture, _renderTexture);
        }

        private void UpdateFog()
        {
            if (_nextUpdateTime > Time.time)
                return;
        
            RenderTexture currentRT = RenderTexture.active;  			
            RenderTexture.active = _renderTexture;
            
            _progressFogTexture.ReadPixels(_rectTexture, 0, 0, false); 			
            _progressFogTexture.Apply();
            
            RenderTexture.active = currentRT;
        
            SaveFog();

            _nextUpdateTime = Time.time + _delayUpdate;
        }

        private void SaveFog()
        {
            byte[] bytes = _progressFogTexture.EncodeToPNG();
            string stringFog = Convert.ToBase64String(bytes);
        
            PlayerPrefs.SetString("Fog", stringFog);
        }
    }
}
