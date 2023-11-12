using UnityEngine;

namespace WiiFly.Camera.Mode {
    public class OrbitMode : ICameraMode {
        #region Fields
        private Transform _cameraTransform;
        private Vector3 _targetPosition;
        #endregion
        
        #region Public Methods
        public void Initialize(UnityEngine.Camera camera) {
            _cameraTransform = camera.transform;
            // TODO: Target position should be the target point of a raycast in the center of the screen
        }
        
        public void Update(Vector2 cursorPosition, float intensity) {}
        
        public string GetModeName() {
            return "Orbit";
        }
        #endregion
    }
}