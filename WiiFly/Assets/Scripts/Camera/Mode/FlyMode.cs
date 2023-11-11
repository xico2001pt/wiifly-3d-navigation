using System;
using UnityEngine;

namespace WiiFly.Camera.Mode {
    public class FlyMode : ICameraMode {
        #region Fields
        private float _maxAngularSpeed = 90f;  // TODO: Singleton class should host parameters like this
        private float _maxLinearSpeed = 25f;
        
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
            float angularSpeedX = _maxAngularSpeed * cursorPosition.x;
            float angularSpeedY = _maxAngularSpeed * cursorPosition.y;
            
            // Update local data of camera rotation
            _cameraRotationEuler.x += angularSpeedY * Time.deltaTime;
            _cameraRotationEuler.y += angularSpeedX * Time.deltaTime;
            
            // Clamp vertical camera rotation
            _cameraRotationEuler.x = Mathf.Clamp(_cameraRotationEuler.x, -90f, 90f);
            
            // Update camera rotation
            _cameraTransform.rotation = Quaternion.Euler(_cameraRotationEuler.x, _cameraRotationEuler.y, _cameraRotationEuler.z);
        }

        private void UpdateCameraPosition(float intensity) {
            // Calculate linear speed
            float linearSpeed = _maxLinearSpeed * intensity;

            // Update camera position
            _cameraTransform.position += linearSpeed * Time.deltaTime * _cameraTransform.forward;
        }
        #endregion
    }
}