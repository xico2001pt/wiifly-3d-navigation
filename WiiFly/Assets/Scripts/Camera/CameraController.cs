using System;
using UnityEngine;
using WiiFly.Camera.Mode;
using WiiFly.Cursor;
using WiiFly.GUI;

namespace WiiFly.Camera {
    public class CameraController : MonoBehaviour {
        #region Events
        public event EventHandler<string> OnUpdateCameraMode; 
        #endregion
        
        #region Fields
        private static readonly ICameraMode[] CameraModes = {
            new FlyMode(),
            //new OrbitMode()
        };
        
        [SerializeField] private RotationGridController rotationGridController;
        [SerializeField, Range(0, 1)] private float deadZoneIntensityRange = 0.1f;

        private UnityEngine.Camera _camera;
        private CursorController _cursorController;
        private Vector2 _deadZonePositionRange;
        private ICameraMode _cameraMode;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _camera = GetComponent<UnityEngine.Camera>();
            _cursorController = FindObjectOfType<CursorController>();  // TODO: May go to inspector
            _deadZonePositionRange = rotationGridController.GetDeadZoneRatio() * 2f;
        }

        protected void Start() {
            SetCameraMode(0);
        }

        private void LateUpdate() {
            // Get current cursor position
            Vector2 cursorPosition = _cursorController.GetCursorPosition();
            
            // Process position dead zone
            cursorPosition.x = NormalizeDeadZonedValue(cursorPosition.x, _deadZonePositionRange.x);
            cursorPosition.y = NormalizeDeadZonedValue(cursorPosition.y, _deadZonePositionRange.y);
            
            // Get current cursor intensity
            float cursorIntensity = _cursorController.GetCursorIntensity();
            
            // Process intensity dead zone
            cursorIntensity = NormalizeDeadZonedValue(cursorIntensity, deadZoneIntensityRange);
            
            // Send cursor position to camera mode
            _cameraMode.Update(cursorPosition, cursorIntensity);
        }
        #endregion

        #region Private Methods
        private void SetCameraMode(int index) {
            _cameraMode = CameraModes[index];
            _cameraMode.Initialize(_camera);
            OnUpdateCameraMode?.Invoke(this, _cameraMode.GetModeName());
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
