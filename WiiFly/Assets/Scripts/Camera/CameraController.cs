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
        private Vector2 _deadZoneRatio;
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _cursorController = FindObjectOfType<CursorController>();
            _deadZoneRatio = rotationGridController.GetDeadZoneRatio();
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = _cursorController.GetCursorPosition();
            float intensity = _cursorController.GetCursorIntensity();

            Vector2 angularSpeed = GetAngularSpeed(cursorPosition);

            float rotationX = transform.rotation.eulerAngles.x + angularSpeed.y * Time.deltaTime;
            float rotationY = transform.rotation.eulerAngles.y + angularSpeed.x * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

            float linearSpeed = CalculateLinearSpeed(intensity);

            transform.position += linearSpeed * Time.deltaTime * transform.forward;
        }
        #endregion

        #region Private Methods
        private float CalculateLinearSpeed(float intensity) {
            float linearSpeed = maxLinearSpeed * (intensity - 0.5f) * 2f;
            return linearSpeed;
        }
        
        private Vector2 GetAngularSpeed(Vector2 cursorPosition) {
            // Change origin to zero
            Vector2 normalizedCursorPosition = cursorPosition - new Vector2(0.5f, 0.5f);
            
            // Process dead zone
            if (Mathf.Abs(normalizedCursorPosition.x) < _deadZoneRatio.x) {
                normalizedCursorPosition.x = 0;
            }
            if (Mathf.Abs(normalizedCursorPosition.y) < _deadZoneRatio.y) {
                normalizedCursorPosition.y = 0;
            }
            
            // Change scale to [-1, 1]
            normalizedCursorPosition *= 2f;

            // Calculate angular velocity
            float angularSpeedX = normalizedCursorPosition.x * maxAngularSpeed;
            float angularSpeedY = normalizedCursorPosition.y * maxAngularSpeed;

            return new Vector2(angularSpeedX, angularSpeedY);
        }
        #endregion
    }
}
