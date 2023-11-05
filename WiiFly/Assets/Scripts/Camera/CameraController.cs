using System;
using UnityEngine;
using WiiFly.Cursor;
using WiiFly.GUI;

namespace WiiFly.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private RotationGridController rotationGridController;
        [SerializeField] private float maxAngularSpeed = 90f;
        [SerializeField] private float maxLinearSpeed = 25f;
        [SerializeField, Range(0, 1)] private float deadZoneIntensityRange = 0.1f;

        private CursorController _cursorController;
        private Vector2 _deadZonePositionRange;
        private Vector3 _cameraRotationEuler;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _cursorController = FindObjectOfType<CursorController>();
            _deadZonePositionRange = rotationGridController.GetDeadZoneRatio() * 2f;
        }

        protected void Start() {
            _cameraRotationEuler = transform.rotation.eulerAngles;
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = _cursorController.GetCursorPosition();
            float intensity = _cursorController.GetCursorIntensity();

            Vector2 angularSpeed = CalculateAngularSpeed(cursorPosition);

            _cameraRotationEuler.x += angularSpeed.y * Time.deltaTime;
            _cameraRotationEuler.y += angularSpeed.x * Time.deltaTime;
            
            _cameraRotationEuler.x = Mathf.Clamp(_cameraRotationEuler.x, -90f, 90f);
            
            transform.rotation = Quaternion.Euler(_cameraRotationEuler.x, _cameraRotationEuler.y, _cameraRotationEuler.z);
            
            float linearSpeed = CalculateLinearSpeed(intensity);

            transform.position += linearSpeed * Time.deltaTime * transform.forward;
        }
        #endregion

        #region Private Methods
        private float CalculateLinearSpeed(float intensity) {
            // Process dead zone
            intensity = NormalizeDeadZonedValue(intensity, deadZoneIntensityRange);

            // Calculate linear speed
            return maxLinearSpeed * intensity;
        }
        
        private Vector2 CalculateAngularSpeed(Vector2 cursorPosition) {
            // Process dead zone
            cursorPosition.x = NormalizeDeadZonedValue(cursorPosition.x, _deadZonePositionRange.x);
            cursorPosition.y = NormalizeDeadZonedValue(cursorPosition.y, _deadZonePositionRange.y);
            
            // Calculate angular speed
            float angularSpeedX = maxAngularSpeed * cursorPosition.x;
            float angularSpeedY = maxAngularSpeed * cursorPosition.y;

            return new Vector2(angularSpeedX, angularSpeedY);
        }

        private float NormalizeDeadZonedValue(float value, float deadZoneRatio) {
            if (Mathf.Abs(value) < deadZoneRatio) {
                return 0;
            }
            return Mathf.Max(Mathf.Abs(value) - deadZoneRatio, 0) / (1f - deadZoneRatio) * Mathf.Sign(value);
        }
        #endregion
    }
}
