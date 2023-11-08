using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiFly.Cursor;
using WiimoteApi;

namespace WiiFly.Input
{
    public class WiimoteController : MonoBehaviour
    {
        #region Fields
        private Wiimote _wiimote;
        [SerializeField] private CursorController cursorController;
        #endregion

        private void Start()
        {
            WiimoteManager.FindWiimotes();
            _wiimote = WiimoteManager.Wiimotes[0];
            _wiimote.SendDataReportMode(InputDataType.REPORT_INTERLEAVED);
            _wiimote.SendPlayerLED(true, false, false, false);
            _wiimote.SetupIRCamera(IRDataType.FULL);
        }

        private void OnDestroy()
        {
            if ( _wiimote != null )
            {
                _wiimote.SendPlayerLED(false, true, false, false);
                //WiimoteManager.Cleanup(_wiimote);
            }
        }

        private void Update()
        {
            if ( _wiimote != null )
            {
                int ret;
                do
                {
                    ret = _wiimote.ReadWiimoteData();
                    if (ret > 0)
                    {
                        var irp = _wiimote.Ir.GetIRMidpoint(true);
                        Debug.Log(irp[0].ToString() + " " + irp[1].ToString());
                        if (irp[0] > 0 && irp[1] > 0)
                        {
                            float x = Mathf.Clamp(-(irp[0] * 2 - 1), -1f, 1f);
                            float y = Mathf.Clamp(irp[1] * 2 - 1, -1f, 1f);
                            cursorController.SetCursorData(x, y);
                        }
                    }
                } while ( ret > 0 );
            }
        }
    }
}
