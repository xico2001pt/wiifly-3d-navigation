using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace WiiFly.Camera.Mode {
    [Serializable]
    public class FlyMode : ICameraMode {
        #region Fields
        [SerializeField] private float maxAngularSpeed = 90f;
        [SerializeField] private float maxLinearSpeed = 25f;
        
        private Transform _cameraTransform;
        private Vector3 _cameraRotationEuler;
        #endregion

        #region Public Methods
        public void Initialize(UnityEngine.Camera camera) {
            _cameraTransform = camera.transform;
            _cameraRotationEuler = _cameraTransform.rotation.eulerAngles;
        }

        public void Update(Vector2 cursorPosition, float intensity) {
            UpdateCameraRotation(cursorPosition);
            UpdateCameraPosition(intensity);
        }
        
        public string GetModeName() {
            return "Fly";
        }
        #endregion
        
        #region Private Methods
        private void UpdateCameraRotation(Vector2 cursorPosition) {
            // Calculate angular speed
            float angularSpeedX = maxAngularSpeed * cursorPosition.x;
            float angularSpeedY = maxAngularSpeed * cursorPosition.y;
            
            // Update local data of camera rotation
            _cameraRotationEuler.x += angularSpeedY * Time.deltaTime;
            _cameraRotationEuler.y += angularSpeedX * Time.deltaTime;
            
            // Clamp vertical camera rotation
            _cameraRotationEuler.x = Mathf.Clamp(_cameraRotationEuler.x, -90f, 90f);
            
            // Update camera rotation
            _cameraTransform.rotation = Quaternion.Euler(_cameraRotationEuler);
        }

        private void UpdateCameraPosition(float intensity) {
            // Calculate linear speed
            float linearSpeed = maxLinearSpeed * intensity;

            // Update camera position
            _cameraTransform.position += linearSpeed * Time.deltaTime * _cameraTransform.forward;
        }
        #endregion
    }
}