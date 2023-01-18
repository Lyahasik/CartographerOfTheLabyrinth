using System;
using System.Collections;
using System.Collections.Generic;
using CartographerOfTheLabyrinth.Environment.Level.Teleport;
using CartographerOfTheLabyrinth.Gameplay.Player;
using CartographerOfTheLabyrinth.UI.Map.Icons;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace CartographerOfTheLabyrinth.UI.Map
{
    public class Map : MonoBehaviour
    {
        [SerializeField] private ArrowPlayer _arrow;
    
        [SerializeField] private int[] _sizes;

        [SerializeField] private UnityEvent<float> _onSliderValue;
    
        private MapSettings _settings;
        private PlayerMovement _player;
        private TeleportHandler _teleportHandler;
        private TeleportIconFactory _teleportIconFactory;

        private RectTransform _rectTransform;
        private Vector2 _initialSize;
        private int _currentIdSize;

        private List<TeleportIcon> _teleportIcons = new ();

        private bool _isFirstStart;
    
        [Inject]
        public void Consrtuct(MapSettings settings,
            PlayerMovement playerMovement,
            TeleportHandler teleportHandler,
            TeleportIconFactory teleportIconFactory)
        {
            _settings = settings;
            _player = playerMovement;
            _teleportHandler = teleportHandler;
            _teleportIconFactory = teleportIconFactory;
        }

        private void Awake()
        {
            _rectTransform = transform as RectTransform;

            _initialSize = _rectTransform.sizeDelta;
            _currentIdSize = 0;
        }

        private void Start()
        {
            CreateTeleportIcons();
        }

        private void OnEnable()
        {
            StartCoroutine(Preparation());
        }

        private void OnDisable()
        {
            _rectTransform.sizeDelta = _initialSize;
        }

        private IEnumerator Preparation()
        {
            yield return new WaitForEndOfFrame();
        
            Vector3 playerPosition = new Vector3(-_player.transform.position.x, -_player.transform.position.z, 0f);
            _rectTransform.localPosition = playerPosition * _settings.PixelsPerUnit;
        
            SetZoom(0);
        }

        private void CreateTeleportIcons()
        {
            TeleportData[] teleportsData = _teleportHandler.TeleportsData;

            foreach (TeleportData teleportData in teleportsData)
            {
                TeleportIcon teleportIcon = _teleportIconFactory.Create(
                    transform,
                    teleportData.LevelId,
                    teleportData.Position);

                // teleportIcon.SetParent(transform);
            
                _teleportIcons.Add(teleportIcon);
            }
        }

        public void ZoomOut()
        {
            _currentIdSize = Math.Clamp(_currentIdSize - 1, 0, _sizes.Length - 1);
            _onSliderValue?.Invoke(_currentIdSize);
        }

        public void ZoomIn()
        {
            _currentIdSize = Math.Clamp(_currentIdSize + 1, 0, _sizes.Length - 1);
            _onSliderValue?.Invoke(_currentIdSize);
        }

        public void SetZoom(float id)
        {
            _currentIdSize = (int)id;
            _onSliderValue?.Invoke(_currentIdSize);
        
            Resize();
        }

        private void Resize()
        {
            float previousMultiplier = _sizes[0] / _rectTransform.sizeDelta.x;
        
            _rectTransform.sizeDelta = new Vector2(_sizes[_currentIdSize], _sizes[_currentIdSize]);

            float currentMultiplier = _sizes[0] / _rectTransform.sizeDelta.x;

            float differenceMultiplier = previousMultiplier / currentMultiplier;
            _rectTransform.localPosition *= differenceMultiplier;
        
            _arrow.Resize(differenceMultiplier);
        
            foreach (TeleportIcon teleportIcon in _teleportIcons)
            {
                teleportIcon.Resize(differenceMultiplier);
            }
        }
    }
}
