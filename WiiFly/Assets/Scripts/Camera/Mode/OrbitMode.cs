using System;
using UnityEngine;

namespace WiiFly.Camera.Mode {
    [Serializable]
    public class OrbitMode : ICameraMode {
        #region Fields
        [SerializeField] private float maxAngularSpeed = 90f;
        [SerializeField] private float maxLinearSpeed = 25f;
        
        private Transform _cameraTransform;
        private Vector3 _targetPosition;
        #endregion
        
        #region Public Methods
        public void Initialize(UnityEngine.Camera camera) {
            _cameraTransform = camera.transform;
            if (RaycastFromCamera(camera, out RaycastHit hit)) {
                _targetPosition = hit.point;
            }
            // TODO: ADD TARGET CUBE TO SCENE
        }

        public void Deinitialize() {
        // TODO: REMOVE TARGET CUBE TO SCENE
        }

        public void Update(Vector2 cursorPosition, float intensity) {
            _cameraTransform.RotateAround(_targetPosition, Vector3.up, -maxAngularSpeed * cursorPosition.x * Time.deltaTime);
            _cameraTransform.RotateAround(_targetPosition, _cameraTransform.right, -maxAngularSpeed * cursorPosition.y * Time.deltaTime);
            _cameraTransform.position += maxLinearSpeed * intensity * Time.deltaTime * _cameraTransform.forward;
        }
        
        public string GetModeName() {
            return "Orbit";
        }

        public bool CanInitialize(UnityEngine.Camera camera) {
            return RaycastFromCamera(camera, out RaycastHit _);
        }
        #endregion
        
        #region Private Methods
        private bool RaycastFromCamera(UnityEngine.Camera camera, out RaycastHit hit) {
            Transform cameraTransform = camera.transform;
            return Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit);
        }
        #endregion
    }
}