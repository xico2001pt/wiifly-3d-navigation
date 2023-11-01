using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace WiiFly.GUI {
    public class RotationGridController : MonoBehaviour {
        #region Fields
        /**
         * The radius of the circle in pixels that representing the dead zone.
         */
        [SerializeField] private int circleRadius = 100;
        [SerializeField] private CanvasScaler canvasScaler;
        
        private UILineRenderer _circleLineRenderer;
        private UILineRenderer _leftLineRenderer;
        private UILineRenderer _rightLineRenderer;
        private UILineRenderer _topLineRenderer;
        private UILineRenderer _bottomLineRenderer;
        #endregion
        
        #region Unity Methods

        protected void Awake() {
            UILineRenderer[] lineRenderers = GetComponentsInChildren<UILineRenderer>();
            _circleLineRenderer = lineRenderers[0];
            _leftLineRenderer = lineRenderers[1];
            _rightLineRenderer = lineRenderers[2];
            _topLineRenderer = lineRenderers[3];
            _bottomLineRenderer = lineRenderers[4];
        }

        protected void Start() {
            UpdateLinePoints(_leftLineRenderer, Vector2.left);
            UpdateLinePoints(_rightLineRenderer, Vector2.right);
            UpdateLinePoints(_topLineRenderer, Vector2.up);
            UpdateLinePoints(_bottomLineRenderer, Vector2.down);
            
            UpdateCirclePoints(100);
        }
        #endregion
        
        #region Public Methods
        public Vector2 GetDeadZoneRatio() {
            return new Vector2(circleRadius / canvasScaler.referenceResolution.x, circleRadius / canvasScaler.referenceResolution.y);
        }
        #endregion
        
        #region Private Methods
        private void UpdateLinePoints(UILineRenderer lineRenderer, Vector2 direction) {
            lineRenderer.Points[0] = Vector2.zero + direction * circleRadius;
            lineRenderer.Points[1] = direction * canvasScaler.referenceResolution / 2;
            lineRenderer.SetAllDirty();
        }
        
        private void UpdateCirclePoints(int steps) {
            _circleLineRenderer.Points = new Vector2[steps + 1];
            for (int i = 0; i <= steps; ++i) {
                float angle = i * Mathf.PI * 2 / steps;
                _circleLineRenderer.Points[i] = new Vector2(
                    Mathf.Cos(angle) * circleRadius,
                    Mathf.Sin(angle) * circleRadius
                );
            }
        }
        #endregion
    }
}
