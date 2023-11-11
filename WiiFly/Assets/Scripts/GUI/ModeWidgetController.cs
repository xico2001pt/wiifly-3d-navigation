using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WiiFly.GUI {
    public class ModeWidgetController : MonoBehaviour {
        #region Fields
        [SerializeField] private Sprite flyModeSprite;
        [SerializeField] private Sprite orbitModeSprite;
        
        private TMP_Text _text;
        private Image _image;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _text = GetComponentInChildren<TMP_Text>();
            Image[] images = GetComponentsInChildren<Image>();
            _image = images[1];
        }
        #endregion

        #region Private Methods
        private void ChangeToOrbitMode() {
            SetMode("Orbit", orbitModeSprite);
        }

        private void ChangeToFlyMode() {
            SetMode("Fly", flyModeSprite);
        }
        
        private void SetMode(string mode, Sprite sprite) {
            _text.text = mode + " Mode";
            _image.sprite = sprite;
        }
        #endregion
    }
}
