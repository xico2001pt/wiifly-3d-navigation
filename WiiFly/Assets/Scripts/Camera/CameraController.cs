using UnityEngine;
using WiiFly.Cursor;

namespace WiiFly.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float maxAngularSpeed = 90f;
        [SerializeField] private float maxLinearSpeed = 25f;

        private CursorController _cursorController;
        private VelocityBarController _velocityBarController;
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _cursorController = FindObjectOfType<CursorController>();
            _velocityBarController = FindObjectOfType<VelocityBarController>();
            _velocityBarController.SetMaxSpeed(maxLinearSpeed);
            _velocityBarController.SetMinSpeed(-maxLinearSpeed);
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = _cursorController.GetCursorPosition();
            float intensity = _cursorController.GetCursorIntensity();

            float angularSpeedX = (cursorPosition.x - 0.5f) * maxAngularSpeed; 
            float angularSpeedY = (cursorPosition.y - 0.5f) * maxAngularSpeed;

            float rotationX = transform.rotation.eulerAngles.x + angularSpeedY * Time.deltaTime;
            float rotationY = transform.rotation.eulerAngles.y + angularSpeedX * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

            float linearSpeed = CalculateLinearSpeed(intensity);

            _velocityBarController.SetSpeed(linearSpeed);

            transform.position += linearSpeed * Time.deltaTime * transform.forward;
        }
        #endregion

        private float CalculateLinearSpeed(float intensity)
        {
            float linearSpeed = maxLinearSpeed * (intensity - 0.5f) * 2f;
            return linearSpeed;
        }
    }
}
