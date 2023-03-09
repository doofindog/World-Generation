using UnityEngine;

namespace SingularityGroup.HotReload.Demo {
    public interface IDemo {
        bool IsServerRunning();
        void OpenHotReloadWindow();
        void OpenScriptFile(TextAsset textAsset);
    }
    
    public class FallbackDemo : IDemo {
        public bool IsServerRunning() {
            return true;
        }

        public void OpenHotReloadWindow() {
            //no-op
        }

        public void OpenScriptFile(TextAsset textAsset) {
            //no-op
        }
    }
}