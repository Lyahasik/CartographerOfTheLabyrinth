using Environment;
using UnityEngine;
using Zenject;

namespace Installers.SO
{
    [CreateAssetMenu(fileName = "EnvironmentSettingsInstaller", menuName = "Installers/EnvironmentSettingsInstaller")]
    public class EnvironmentSettingsInstaller : ScriptableObjectInstaller<EnvironmentSettingsInstaller>
    {
        public EnvironmentSettings EnvironmentSettings;
    
        public override void InstallBindings()
        {
            Container.BindInstance(EnvironmentSettings);
        }
    }
}