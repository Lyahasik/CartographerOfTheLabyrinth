using System;
using UnityEngine;
using Zenject;

using Gameplay.Progress;

namespace Gameplay.FogOfWar
{
    public class FogOfWar : MonoBehaviour
    {
        private GameplaySettings _gameplaySettings;
        private ProcessingProgress _processingProgress;
        
        [SerializeField] private RenderTexture _renderTexture;

        private Texture2D _progressFogTexture;
        private Rect _rectTexture;

        private bool _isLoaded;
        private float _nextUpdateTime;

        public static Action<float> OnProgressPercentage;

        [Inject]
        public void Construct(GameplaySettings gameplaySettings, ProcessingProgress processingProgress)
        {
            _gameplaySettings = gameplaySettings;
            _processingProgress = processingProgress;
        }

        private void Start()
        {
            _progressFogTexture = new Texture2D(_renderTexture.width, _renderTexture.height);
            _rectTexture = new Rect(0, 0, _renderTexture.width, _renderTexture.height);
        }

        private void Update()
        {
            UpdateFog();
        }

        public void LoadFog(string stringFog)
        {
            Texture2D texture = new Texture2D(_renderTexture.width, _renderTexture.height);
            _isLoaded = true;

            if (stringFog == string.Empty)
                return;
        
            byte[] bytes = Convert.FromBase64String(stringFog);

            texture.LoadImage(bytes);
        
            Graphics.Blit(texture, _renderTexture);
            CalculateProgress();
        }

        private void UpdateFog()
        {
            if (!_isLoaded
                || _nextUpdateTime > Time.time)
                return;

            RenderTexture currentRT = RenderTexture.active;  			
            RenderTexture.active = _renderTexture;
            
            _progressFogTexture.ReadPixels(_rectTexture, 0, 0, false); 			
            _progressFogTexture.Apply();
            
            RenderTexture.active = currentRT;

            CalculateProgress(true);
            byte[] bytes = _progressFogTexture.EncodeToPNG();
            string stringFog = Convert.ToBase64String(bytes);
            _processingProgress.StringFog = stringFog;

            _nextUpdateTime = Time.time + _gameplaySettings.DelaySave;
        }

        private void CalculateProgress(bool isSaving = false)
        {
            int openPixels = 0;
            Color[] pixels = _progressFogTexture.GetPixels();

            foreach (Color pixel in pixels)
            {
                if (pixel.r > 0f)
                    openPixels++;
            }

            float progressPercentage = openPixels / (float) pixels.Length * 100f;
            
            OnProgressPercentage?.Invoke(progressPercentage);
            
            if (isSaving)
                _processingProgress.UpdateLeaderbord((int) (progressPercentage * 100f));
        }
    }
}
