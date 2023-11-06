using System;
using UnityEngine;
using UnityEngine.UI;
using WiiFly.Cursor;

namespace WiiFly.GUI {
    public class VelocityBarController : MonoBehaviour {
        #region Fields
        [SerializeField] private CursorController cursorController;
        
        private Slider _slider;
        private Image _fill;
        #endregion
        
        #region Unity Methods
        protected void Awake() {
            _slider = GetComponent<Slider>();
            _fill = _slider.fillRect.GetComponent<Image>();
        }

        protected void Start() {
            SetMinIntensity(-1f);
            SetMaxIntensity(1f);
        }

        protected void LateUpdate() {
            SetIntensity(cursorController.GetCursorIntensity());
        }

        #endregion

        #region Private Methods
        private void SetMaxIntensity(float intensity) {
            _slider.maxValue = intensity;
        }

        private void SetMinIntensity(float intensity) {
            _slider.minValue = intensity;
        }

        private void SetIntensity(float intensity) {
            _slider.value = intensity;
            UpdateSliderSense();
            _fill.color = Color.Lerp(Color.green, Color.red, Mathf.Abs(intensity));
        }

        private void UpdateSliderSense() {
            _slider.fillRect.anchorMin = new Vector2(Mathf.Clamp(_slider.handleRect.anchorMin.x, 0, 0.5f), 0);
            _slider.fillRect.anchorMax = new Vector2(Mathf.Clamp(_slider.handleRect.anchorMax.x, 0.5f, 1), 1);
        }
        #endregion
    }
}
