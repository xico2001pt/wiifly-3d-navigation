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
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _cursorController = FindObjectOfType<CursorController>();
            _deadZonePositionRange = rotationGridController.GetDeadZoneRatio() * 2f;
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = _cursorController.GetCursorPosition();
            float intensity = _cursorController.GetCursorIntensity();

            Vector2 angularSpeed = CalculateAngularSpeed(cursorPosition);

            float rotationX = transform.rotation.eulerAngles.x + angularSpeed.y * Time.deltaTime;
            float rotationY = transform.rotation.eulerAngles.y + angularSpeed.x * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

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
