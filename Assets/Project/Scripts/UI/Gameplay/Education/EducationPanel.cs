using OS;
using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Education;

namespace UI.Gameplay.Education
{
    public class EducationPanel : MonoBehaviour
    {
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

        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    
        public void ActivateLesson(int id)
        {
            _currentId = id;
        
            _lessonWindows[_currentId].SetActive(true);
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
        }
    }
}
