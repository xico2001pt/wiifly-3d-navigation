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

        private CursorController _cursorController;
        private Vector2 _deadZoneRange;
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _cursorController = FindObjectOfType<CursorController>();
            _deadZoneRange = rotationGridController.GetDeadZoneRatio() * 2f;
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
            float linearSpeed = maxLinearSpeed * intensity;
            return linearSpeed;
        }
        
        private Vector2 CalculateAngularSpeed(Vector2 cursorPosition) {
            // Process dead zone
            if (Mathf.Abs(cursorPosition.x) < _deadZoneRange.x) {
                cursorPosition.x = 0;
            }
            if (Mathf.Abs(cursorPosition.y) < _deadZoneRange.y) {
                cursorPosition.y = 0;
            }

            // Normalize the slope
            // TODO: Check if this can be simplified
            cursorPosition.x = Mathf.Max(Mathf.Abs(cursorPosition.x) - _deadZoneRange.x, 0) / (1f - _deadZoneRange.x) * Mathf.Sign(cursorPosition.x);
            cursorPosition.y = Mathf.Max(Mathf.Abs(cursorPosition.y) - _deadZoneRange.y, 0) / (1f - _deadZoneRange.y) * Mathf.Sign(cursorPosition.y);
            
            // Calculate angular speed
            float angularSpeedX = maxAngularSpeed * cursorPosition.x;
            float angularSpeedY = maxAngularSpeed * cursorPosition.y;

            return new Vector2(angularSpeedX, angularSpeedY);
        }
        #endregion
    }
}
