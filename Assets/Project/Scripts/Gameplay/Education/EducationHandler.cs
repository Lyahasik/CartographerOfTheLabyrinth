using System.Collections.Generic;
using Zenject;

using FiniteStateMachine;
using Gameplay.Progress;
using UI.Gameplay.Education;

namespace Gameplay.Education
{
    public class EducationHandler : IInitializable
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private EducationPanel _educationPanel;
    
        private HashSet<int> _lessons;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            EducationPanel educationPanel)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _educationPanel = educationPanel;
        }
    
        public void Initialize()
        {
            _lessons = _processingProgress.Lessons;
            ActivateLesson(LessonType.Lesson0);
        }

        public void ActivateLesson(LessonType lessonType)
        {
            var id = (int) lessonType;
        
            if (_lessons.Contains(id))
                return;
        
            _gameMashine.Enter(_container.Instantiate<EducationState>());
            _educationPanel.ActivateLesson(id);

            _lessons.Add(id);
            _processingProgress.SaveLessons();
        }
    }
}