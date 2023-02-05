using System.IO;
using UnityEngine;

namespace Generation
{
    public class SaveMap : MonoBehaviour
    {
        private string _savePath;
        [SerializeField] private RenderTexture _renderTexture;

        private void Update()
        {
            _savePath = Application.dataPath + "\\Project\\Resources\\Map.png";
            DumpRenderTexture();
        }

        public void DumpRenderTexture()
        {
            if (!Input.GetKeyDown(KeyCode.F))
                return;
        
            Debug.Log("Save " + _savePath);
        
            var oldRT = RenderTexture.active;

            var tex = new Texture2D(_renderTexture.width, _renderTexture.height);
            RenderTexture.active = _renderTexture;
            tex.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            tex.Apply();

            File.WriteAllBytes(_savePath, tex.EncodeToPNG());
            RenderTexture.active = oldRT;
        }
    }
}
