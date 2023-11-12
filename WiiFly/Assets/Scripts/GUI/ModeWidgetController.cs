using UnityEngine;
using UnityEngine.UI;
using TMPro;
using WiiFly.Camera;

namespace WiiFly.GUI {
    public class ModeWidgetController : MonoBehaviour {
        #region Fields
        [SerializeField] private Sprite flyModeSprite;
        [SerializeField] private Sprite orbitModeSprite;
        [SerializeField] private CameraController cameraController;
        
        private TMP_Text _text;
        private Image _image;
        #endregion

        #region Unity Methods
        protected void Awake() {
            _text = GetComponentInChildren<TMP_Text>();
            Image[] images = GetComponentsInChildren<Image>();
            _image = images[1];
            cameraController.OnUpdateCameraMode += OnUpdateCameraMode;
        }
        #endregion

        #region Private Methods
        private void OnUpdateCameraMode(object sender, string mode) {
            Sprite sprite = null;
            switch (mode) {
                case "Fly":
                    sprite = flyModeSprite;
                    break;
                case "Orbit":
                    sprite = orbitModeSprite;
                    break;
            }
            SetMode(mode, sprite);
        }
        
        private void SetMode(string mode, Sprite sprite) {
            _text.text = mode + " Mode";
            _image.sprite = sprite;
        }
        #endregion
    }
}
