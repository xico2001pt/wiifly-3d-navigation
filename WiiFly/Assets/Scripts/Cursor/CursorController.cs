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
        
        public float GetCursorIntensity()
        {
            return _cursorData.Intensity;
        }
        
        public void SetCursorData(float x, float y) {
            _cursorData.Position.x = x;
            _cursorData.Position.y = y;
            UpdateTransform();
        }

        public void SetCursorIntensity(float intensity)
        {
            _cursorData.Intensity = intensity;
        }
        #endregion

        #region Private Methods
        private void UpdateTransform()
        {
            _cursorRectTransform.anchoredPosition =
                _cursorData.GetNormalizedPosition() *
                canvasScaler.referenceResolution *
                new Vector2(1, -1);  // Y is negative because the y-axis is inverted
        }
        #endregion
    }
}
