using CartographerOfTheLabyrinth.UI.Map;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Installers.SO
{
    [CreateAssetMenu(fileName = "UISettingsInstaller", menuName = "Installers/UISettingsInstaller")]
    public class UISettingsInstaller : ScriptableObjectInstaller<UISettingsInstaller>
    {
        public MapSettings MapSettings;
    
        public override void InstallBindings()
        {
            Container.BindInstance(MapSettings);
        }
    }
}