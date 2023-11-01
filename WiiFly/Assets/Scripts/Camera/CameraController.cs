using UnityEngine;
using WiiFly.Cursor;

namespace WiiFly.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float maxAngularSpeed = 90f;
        [SerializeField] private float maxLinearSpeed = 25f;
        [SerializeField] private float middleDistance = 5f;
        [SerializeField, Range(1, 10)] private float distance = 5.0f;

        private CursorController _cursorController;
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _cursorController = FindObjectOfType<CursorController>();
        }

        private void LateUpdate()
        {
            Vector2 cursorPosition = _cursorController.GetCursorPosition();

            float angularSpeedX = (cursorPosition.x - 0.5f) * maxAngularSpeed; 
            float angularSpeedY = (cursorPosition.y - 0.5f) * maxAngularSpeed;

            float rotationX = transform.rotation.eulerAngles.x + angularSpeedY * Time.deltaTime;
            float rotationY = transform.rotation.eulerAngles.y + angularSpeedX * Time.deltaTime;

            transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);

            float linearSpeed = CalculateLinearSpeed(distance);

            transform.position += linearSpeed * Time.deltaTime * transform.forward;
        }
        #endregion

        private float CalculateLinearSpeed(float distance)
        {
            float linearSpeed;
            if (distance == middleDistance)
            {
                linearSpeed = 0f;
            }
            else if (distance < middleDistance)
            {
                linearSpeed = maxLinearSpeed / distance;
            }
            else
            {
                linearSpeed = -maxLinearSpeed / (11 - distance);
            }

            return linearSpeed;
        }
    }
}
