using System.Collections;
using System.IO;
using SingularityGroup.HotReload.Demo;
using UnityEditor;
using UnityEngine;

namespace SingularityGroup.HotReload.Editor.Demo {
    class EditorDemo : IDemo {
        public bool IsServerRunning() {
            return ServerHealthCheck.I.IsServerHealthy;
        }

        public void OpenHotReloadWindow() {
            HotReloadWindow.Open();
        }

        public void OpenScriptFile(TextAsset textAsset) {
            var path = Path.GetFullPath(AssetDatabase.GetAssetPath(textAsset));
#if UNITY_2019_4_OR_NEWER
            Unity.CodeEditor.CodeEditor.CurrentEditor.OpenProject(path, 43, 28);
#else
            EditorUtility.OpenWithDefaultApp(path);
#endif
        }
    }
}
