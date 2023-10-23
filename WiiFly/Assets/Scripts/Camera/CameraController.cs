using UnityEngine;
using WiiFly.Cursor;

namespace WiiFly.Camera
{
    public class CameraController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private float maxAngularSpeed = 90f;

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
        }
        #endregion
    }
}
