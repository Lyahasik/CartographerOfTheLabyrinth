using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Education;
using Audio;
using OS;

namespace UI.Gameplay.Education
{
    public class EducationPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        
        private DiContainer _container;
        private GameMashine _gameMashine;
    
        [SerializeField] private GameObject[] _lessonWindows;

        private int _currentId;
    
        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }

        private void Awake()
        {
            transform.SetSiblingIndex(3);
        }

        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    
        public void ActivateLesson(int id)
        {
            _currentId = id;
        
            _lessonWindows[_currentId].SetActive(true);
            AudioHandler.ActivateClip(_paperClipName);
        }

        public void DeactivateLesson()
        {
            _lessonWindows[_currentId].SetActive(false);

            if (_currentId == (int) LessonType.Lesson0)
            {
                ActivateLesson(_currentId + 1);
                return;
            }
            else if (_currentId == (int) LessonType.Lesson1
                     && !OSHandler.IsWebMobile())
            {
                ActivateLesson(_currentId + 1);
                return;
            }
        
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
            AudioHandler.ActivateClip(_paperClipName);
        }
    }
}
