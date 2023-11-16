using UnityEngine;

namespace WiiFly.Camera.Mode {
    public interface ICameraMode {
        // Mandatory methods
        public void Initialize(UnityEngine.Camera camera);
        public void Update(Vector2 cursorPosition, float intensity);
        public string GetModeName();
        
        // Optional methods
        public void Denitialize() {}
        public bool CanInitialize(UnityEngine.Camera camera) { return true; }
    }
}