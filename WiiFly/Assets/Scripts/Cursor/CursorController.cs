using UnityEngine;
using UnityEngine.UI;

namespace WiiFly.Cursor {
    public class CursorController : MonoBehaviour {
        #region Fields
        [SerializeField] private CanvasScaler canvasScaler;
        [SerializeField] private CursorData cursorData;
        
        private RectTransform _cursorRectTransform;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _cursorRectTransform = GetComponent<RectTransform>();
        }
        
        protected void Start() {
            UpdateTransform();
        }
        
        protected void Update() {  // TODO: Remove this method, after implementing the Wiimote update method
            UpdateTransform();
        }
        #endregion
        
        #region Public Methods
        public Vector2 GetCursorPosition() {
            return cursorData.position;
        }
        
        public void SetCursorData(float x, float y) {
            cursorData.position.x = x;
            cursorData.position.y = y;
            UpdateTransform();
        }
        #endregion

        #region Private Methods
        private void UpdateTransform() {
            _cursorRectTransform.anchoredPosition = new Vector2(
                cursorData.position.x * canvasScaler.referenceResolution.x,
                -cursorData.position.y * canvasScaler.referenceResolution.y  // Negative, because the y-axis is inverted
            );
        }
        #endregion
    }
}
