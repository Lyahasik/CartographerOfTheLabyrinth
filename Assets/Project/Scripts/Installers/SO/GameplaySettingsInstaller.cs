using CartographerOfTheLabyrinth.Gameplay;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Installers.SO
{
    [CreateAssetMenu(fileName = "GameplaySettingsInstaller", menuName = "Installers/GameplaySettingsInstaller")]
    public class GameplaySettingsInstaller : ScriptableObjectInstaller<GameplaySettingsInstaller>
    {
        public PlayerSettings PlayerSettings;
    
        public override void InstallBindings()
        {
            Container.BindInstance(PlayerSettings);
        }
    }
}