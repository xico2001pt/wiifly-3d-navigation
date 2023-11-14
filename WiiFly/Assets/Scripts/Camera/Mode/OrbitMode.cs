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
            // TODO: Target position should be the target point of a raycast in the center of the screen
            RaycastHit hit;
            if (Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out hit, Mathf.Infinity))
            {
                Debug.DrawRay(_cameraTransform.position, _cameraTransform.forward * hit.distance, Color.red);
                Debug.Log("start + dir: " + _cameraTransform.position + _cameraTransform.forward * hit.distance);
                Debug.Log("point: " + hit.point);
                Debug.Log("Hit object: " + hit.collider.gameObject.name);
                _targetPosition = hit.point;
            }
        }
        
        public void Update(Vector2 cursorPosition, float intensity) {}
        
        public string GetModeName() {
            return "Orbit";
        }
        #endregion
    }
}