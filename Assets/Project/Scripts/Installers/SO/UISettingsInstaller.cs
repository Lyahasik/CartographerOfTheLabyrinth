using UnityEngine;
using Zenject;

using UI.Map;

namespace Installers.SO
{
    [CreateAssetMenu(fileName = "UISettingsInstaller", menuName = "Installers/UISettingsInstaller")]
    public class UISettingsInstaller : ScriptableObjectInstaller<UISettingsInstaller>
    {
        public UISettings _uiSettings;
    
        public override void InstallBindings()
        {
            Container.BindInstance(_uiSettings);
        }
    }
}