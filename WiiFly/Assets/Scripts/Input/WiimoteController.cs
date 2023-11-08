using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using WiiFly.Cursor;
using WiimoteApi;

namespace WiiFly.Input {
    public class WiimoteController : MonoBehaviour {
        #region Fields
        [SerializeField] private CursorController cursorController;
        [SerializeField] private float positionInterpolationSpeed = 5f;
        [SerializeField] private float barZoom = 1.2f;
        [SerializeField] private float neutralLinearSpeed = 40f;
        [SerializeField] private float linearSpeedRange = 10f;
        [SerializeField] private float intensityInterpolationSpeed = 5f;
        
        private Wiimote _wiimote;
        private float _xPosition, _yPosition;
        private float _targetXPosition, _targetYPosition;
        
        private float _linearSpeed;
        private float _targetLinearSpeed;
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
                        UpdateCursorIntensity();
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
            } else {
                _targetXPosition = 0f;
                _targetYPosition = 0f;
            }
            
            _xPosition = InterpolateCursorValue(_xPosition, _targetXPosition);
            _yPosition = InterpolateCursorValue(_yPosition, _targetYPosition);
            cursorController.SetCursorData(_xPosition, _yPosition);
        }
        
        private float InterpolateCursorValue(float currentValue, float targetValue) {
            return Mathf.Lerp(currentValue, targetValue, positionInterpolationSpeed * Time.deltaTime);
        }
        
        private float InterpolateCursorIntensity(float currentValue, float targetValue) {
            return Mathf.Lerp(currentValue, targetValue, intensityInterpolationSpeed * Time.deltaTime);
        }
        
        private float ApplyBarZoom(float value) {
            return value * barZoom;
        }
        
        private void UpdateCursorIntensity() {
            float intensity = GetAveragePointsIntensity();
            if (intensity > -1) {
                _targetLinearSpeed = Mathf.Clamp(intensity, neutralLinearSpeed - linearSpeedRange, neutralLinearSpeed + linearSpeedRange);
            } else {  // Stop if not detected
                _targetLinearSpeed = neutralLinearSpeed;
            }
            _linearSpeed = InterpolateCursorIntensity(_linearSpeed, _targetLinearSpeed);
            
            intensity = CursorData.GetNormalizedValue(_linearSpeed, neutralLinearSpeed - linearSpeedRange, neutralLinearSpeed + linearSpeedRange);
            intensity = intensity * 2 - 1;
            cursorController.SetCursorIntensity(intensity);
        }
        
        private float GetAveragePointsIntensity() {
            float sum = 0f;
            int count = 0;
            for (int i = 0; i < 4; ++i) {
                int value = _wiimote.Ir.ir[i, 7];
                if (value > -1) {
                    sum += value;
                    ++count;
                }
            }

            if (count == 0)
                return -1;
            return sum / count;
        }
        #endregion
    }
}
