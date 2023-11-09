using UnityEngine;
using WiiFly.Cursor;

namespace WiiFly.Input {
    public class DebugInputController : MonoBehaviour {
        #region Fields
        [SerializeField] private CursorController cursorController;
        [SerializeField, Range(-1, 1)] private float cursorX;
        [SerializeField, Range(-1, 1)] private float cursorY;
        [SerializeField, Range(-1, 1)] private float intensity;
        #endregion

        #region Unity Methods
        private void Start() {
            UpdateCursorData();
        }

        private void Update() {
            UpdateCursorData();
        }

        #endregion
        
        #region Private Methods
        private void UpdateCursorData() {
            cursorController.SetCursorData(cursorX, cursorY);
            cursorController.SetCursorIntensity(intensity);
        }
        #endregion
    }
}
