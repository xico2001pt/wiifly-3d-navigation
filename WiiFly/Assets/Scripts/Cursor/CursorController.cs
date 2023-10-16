using UnityEngine;
using UnityEngine.UI;

namespace WiiFly.Cursor {
    public class CursorController : MonoBehaviour {
        #region Fields
        [SerializeField] private CanvasScaler canvasScaler;

        private RectTransform _cursorRectTransform;
        private CursorData _cursorData;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _cursorRectTransform = GetComponent<RectTransform>();
            _cursorData = new CursorData();
        }
        
        protected void Start() {
            UpdateTransform();
        }
        #endregion
        
        #region Public Methods
        public Vector2 GetCursorPosition() {
            return _cursorData.Position;
        }
        
        // TODO: Get Cursor intensity
        
        public void SetCursorData(float x, float y) {
            _cursorData.Position.x = x;
            _cursorData.Position.y = y;
            // TODO: Set Cursor intensity
            UpdateTransform();
        }
        #endregion

        #region Private Methods
        private void UpdateTransform() {
            _cursorRectTransform.anchoredPosition = new Vector2(
                _cursorData.Position.x * canvasScaler.referenceResolution.x,
                -_cursorData.Position.y * canvasScaler.referenceResolution.y  // Negative, because the y-axis is inverted
            );
        }
        #endregion
    }
}
