using System;
using UnityEngine;
using Zenject;

using Gameplay.Progress;

namespace Gameplay.FogOfWar
{
    public class FogOfWar : MonoBehaviour
    {
        private ProcessingProgress _processingProgress;
        
        [SerializeField] private RenderTexture _renderTexture;

        private Texture2D _progressFogTexture;
        private Rect _rectTexture;

        private float _delayUpdate = 3f;
        private float _nextUpdateTime;

        public static Action<float> OnProgressPercentage;

        [Inject]
        public void Construct(ProcessingProgress processingProgress)
        {
            _processingProgress = processingProgress;
        }

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

            string stringFog = _processingProgress.StringFog;

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

            CalculateProgress();
            _processingProgress.SaveFog(_progressFogTexture);

            _nextUpdateTime = Time.time + _delayUpdate;
        }

        private void CalculateProgress()
        {
            int openPixels = 0;
            Color[] pixels = _progressFogTexture.GetPixels();

            foreach (Color pixel in pixels)
            {
                if (pixel.r > 0f)
                    openPixels++;
            }

            float progressPercentage = openPixels / (float) pixels.Length * 100f;
            
            _processingProgress.UpdateLeaderbord((int) (progressPercentage * 100f));
            OnProgressPercentage?.Invoke(progressPercentage);
        }
    }
}
