using UI.Map;
using UnityEngine;
using Zenject;

namespace Installers.SO
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