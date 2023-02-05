using Gameplay;
using UnityEngine;
using Zenject;

namespace Installers.SO
{
    [CreateAssetMenu(fileName = "GameplaySettingsInstaller", menuName = "Installers/GameplaySettingsInstaller")]
    public class GameplaySettingsInstaller : ScriptableObjectInstaller<GameplaySettingsInstaller>
    {
        public GameplaySettings _gameplaySettings;
    
        public override void InstallBindings()
        {
            Container.BindInstance(_gameplaySettings);
        }
    }
}