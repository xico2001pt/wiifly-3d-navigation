using System;
using UnityEngine;

namespace WiiFly.Camera.Mode {
    [Serializable]
    public class OrbitMode : ICameraMode {
        #region Fields
        [SerializeField] private float maxHorizontalAngularSpeed = 90f;
        [SerializeField] private float maxVerticalAngularSpeed = 45f;
        [SerializeField] private float maxLinearSpeed = 25f;
        [SerializeField] private float minDistance = 0.3f;
        
        private Transform _cameraTransform;
        private Vector3 _targetPosition;
        private GameObject _targetCube;
        #endregion

        #region Public Methods
        public void Initialize(UnityEngine.Camera camera) {
            _cameraTransform = camera.transform;
            if (RaycastFromCamera(camera, out RaycastHit hit)) {
                _targetPosition = hit.point;
                _targetCube = CreateCube(_targetPosition, Color.red);
            }
        }

        public void Deinitialize() {
            if (_targetCube != null) {
                GameObject.Destroy(_targetCube);
                _targetCube = null;
            }
        }

        public void Update(Vector2 cursorPosition, float intensity) {
            // Calculate rotation angle
            float rotationAngleX = maxHorizontalAngularSpeed * cursorPosition.x * Time.deltaTime;
            float rotationAngleY = maxVerticalAngularSpeed * cursorPosition.y * Time.deltaTime;
            
            // Rotate horizontally around target
            _cameraTransform.RotateAround(_targetPosition, Vector3.up, -rotationAngleX);
            
            // Get distance of vertical rotation to -90 or 90 degrees
            float rotationAngleYSign = Mathf.Sign(rotationAngleY);
            float verticalRotationDistance = 90f;
            float cameraRotationX = _cameraTransform.rotation.eulerAngles.x;
            float maxAngle = 89.8f;
            float minAngle = 270.8f;
            if (rotationAngleYSign < 0) {
                if (cameraRotationX < 90f) {  // Top side
                    verticalRotationDistance = maxAngle - cameraRotationX;
                } else {  // Bottom side
                    verticalRotationDistance = 360f - cameraRotationX + maxAngle;
                }
            } else {
                if (cameraRotationX > 270f) {  // Bottom side
                    verticalRotationDistance = cameraRotationX - minAngle;
                } else {  // Top side
                    verticalRotationDistance = 90f + cameraRotationX;
                }
            }

            // Clamp vertical rotation
            verticalRotationDistance = Mathf.Max(0f, verticalRotationDistance);
            rotationAngleY = Mathf.Min(verticalRotationDistance, Mathf.Abs(rotationAngleY)) * rotationAngleYSign;
            
            // Rotate vertically around target
            _cameraTransform.RotateAround(_targetPosition, _cameraTransform.right, -rotationAngleY);

            // Move forward/backward
            float linearDistance = maxLinearSpeed * intensity * Time.deltaTime;
            float distanceToTarget = Vector3.Distance(_cameraTransform.position, _targetPosition);
            if (linearDistance > 0) {
                linearDistance = Mathf.Min(linearDistance, distanceToTarget - minDistance);
            }
            _cameraTransform.position += linearDistance * _cameraTransform.forward;
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
        
        private Vector3 RotateAround(Vector3 point, Vector3 pivot, Vector3 axis, float angle) {
            Vector3 direction = point - pivot;  // Get direction vector
            direction = Quaternion.AngleAxis(angle, axis) * direction;  // Rotate point
            point = direction + pivot;  // Calculate rotated point
            return point;  // Return it
        }
        
        private GameObject CreateCube(Vector3 position, Color color) {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = position;
            cube.GetComponent<Renderer>().material.color = color;
            cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            return cube;
        }
        #endregion
    }
}