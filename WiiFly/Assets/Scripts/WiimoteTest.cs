using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiimoteApi;

// Test class to get the values form the wiimote motion plus and print them to the console
public class WiimoteTest : MonoBehaviour {
    [SerializeField] private Transform _target, _midpoint;
    private Wiimote _wiimote;
    private bool _wiiMotionPlusActive = false;
   
    void Start() {
        WiimoteManager.FindWiimotes();
        _wiimote = WiimoteManager.Wiimotes[0];
        _wiimote.SendDataReportMode(InputDataType.REPORT_BUTTONS_IR10_EXT9);
        _wiimote.SendPlayerLED(true, false, false, false);
        int z = _wiimote.SendStatusInfoRequest();
        Debug.Log("Status info request: " + z);
        Debug.Log(_wiimote.Status.battery_level);
        _wiiMotionPlusActive = _wiimote.ActivateWiiMotionPlus();
        _wiimote.SetupIRCamera(IRDataType.BASIC);
    }

    private void OnDestroy() {
        if (_wiimote != null) {
            _wiimote.SendPlayerLED(false, true, false, false);
            if (_wiiMotionPlusActive) {
                _wiimote.DeactivateWiiMotionPlus();
            }
            //WiimoteManager.Cleanup(_wiimote);
        }
    }

    void Update() {
        if (_wiimote != null) {
            int ret;
            do
            {
                ret = _wiimote.ReadWiimoteData();
                if (ret > 0)
                {
                    if (_wiimote.current_ext == ExtensionController.MOTIONPLUS)
                    {
                        MotionPlusData data = _wiimote.MotionPlus;
                        float threshold = 0.0045f;
                        Vector3 offset = new Vector3(-data.PitchSpeed, -data.YawSpeed, -data.RollSpeed) / 95f;
                        offset.x = Mathf.Abs(offset.x) > threshold ? offset.x : 0;
                        offset.y = Mathf.Abs(offset.y) > threshold ? offset.y : 0;
                        offset.z = Mathf.Abs(offset.z) > threshold ? offset.z : 0;
                        
                        //Debug.Log(offset.x);
                        transform.Rotate(offset, Space.Self);
                    }

                    var irp = _wiimote.Ir.GetIRMidpoint(true);
                    if (irp[0] > 0 && irp[1] > 0)
                    {
                        _midpoint.transform.localPosition = new Vector3(irp[0] - 0.5f, -(irp[1] - 0.5f), 0.62f);
                        var ir = _wiimote.Ir.ir;
                        Debug.Log("Point1: " + ir[0, 0] + " " + ir[0, 1] + " " + ir[0, 2]);
                        Debug.Log("Point2: " + ir[1, 0] + " " + ir[1, 1] + " " + ir[1, 2]);
                        Debug.Log("Point3: " + ir[2, 0] + " " + ir[2, 1] + " " + ir[2, 2]);
                        Debug.Log("Point4: " + ir[3, 0] + " " + ir[3, 1] + " " + ir[3, 2]);
                    }
                    //Debug.Log(irp[0] + " " + irp[1]);
                    
                    if (_wiimote.Button.a)
                    {
                        Debug.Log("A pressed");
                        Debug.Log(_wiimote.SendStatusInfoRequest());
                    }
                }
            } while (ret > 0);
            
            // If key 1 pressed, calibration step 1 and so on
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _wiimote.Accel.CalibrateAccel(AccelCalibrationStep.A_BUTTON_UP);
                _target.eulerAngles = new Vector3(0, 0, 0);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                _wiimote.Accel.CalibrateAccel(AccelCalibrationStep.EXPANSION_UP);
                _target.eulerAngles = new Vector3(-90, 0, 0);
            }
        
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                _wiimote.Accel.CalibrateAccel(AccelCalibrationStep.LEFT_SIDE_UP);
                _target.eulerAngles = new Vector3(0, 0, 90);
            }

            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                _wiimote.MotionPlus.SetZeroValues();
            }
        }
    }
}
