using UnityEngine;
using WiiFly.Cursor;

namespace WiiFly.Input {
    public class DebugInputController : MonoBehaviour {
        #region Fields
        [SerializeField] private CursorController cursorController;
        [SerializeField, Range(0, 1)] private float cursorX = 0.5f;
        [SerializeField, Range(0, 1)] private float cursorY = 0.5f;
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
        }
        #endregion
    }
}
