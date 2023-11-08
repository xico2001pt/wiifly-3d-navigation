using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiFly.Cursor;
using WiimoteApi;

namespace WiiFly.Input {
    public class WiimoteController : MonoBehaviour {
        #region Fields
        [SerializeField] private CursorController cursorController;
        [SerializeField] private float interpolationSpeed = 5f;
        [SerializeField] private float barZoom = 1.2f;
        
        private Wiimote _wiimote;
        private float _xPosition, _yPosition;
        private float _targetXPosition, _targetYPosition;
        #endregion

        #region Unity Methods
        private void Start() {
            WiimoteManager.FindWiimotes();
            _wiimote = WiimoteManager.Wiimotes[0];
            _wiimote.SendDataReportMode(InputDataType.REPORT_INTERLEAVED);
            _wiimote.SendPlayerLED(true, false, false, false);
            _wiimote.SetupIRCamera(IRDataType.FULL);
        }

        private void OnDestroy() {
            if ( _wiimote != null ) {
                _wiimote.SendPlayerLED(false, true, false, false);
                //WiimoteManager.Cleanup(_wiimote);
            }
        }

        private void Update() {
            if (_wiimote != null) {
                int ret;
                do {
                    ret = _wiimote.ReadWiimoteData();
                    if (ret > 0) {
                        UpdateCursorPosition();
                    }
                } while (ret > 0);
            }
        }
        #endregion
        
        #region Private Methods
        private void UpdateCursorPosition() {
            var irp = _wiimote.Ir.GetIRMidpoint();
            if (irp[0] > 0 && irp[1] > 0) {
                // Convert to [-1, 1] range
                _targetXPosition = -(irp[0] * 2 - 1);
                _targetYPosition = irp[1] * 2 - 1;
                
                // Apply virtual bar zoom
                _targetXPosition = ApplyBarZoom(_targetXPosition);
                _targetYPosition = ApplyBarZoom(_targetYPosition);
                
                // Clamp to [-1, 1] range
                _targetXPosition = Mathf.Clamp(_targetXPosition, -1f, 1f);
                _targetYPosition = Mathf.Clamp(_targetYPosition, -1f, 1f);
            }
            
            _xPosition = InterpolateCursorValue(_xPosition, _targetXPosition);
            _yPosition = InterpolateCursorValue(_yPosition, _targetYPosition);
            cursorController.SetCursorData(_xPosition, _yPosition);
        }
        
        private float InterpolateCursorValue(float currentValue, float targetValue) {
            return Mathf.Lerp(currentValue, targetValue, interpolationSpeed * Time.deltaTime);
        }
        
        private float ApplyBarZoom(float value) {
            return value * barZoom;
        }
        #endregion
    }
}
