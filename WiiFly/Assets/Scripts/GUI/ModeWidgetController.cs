using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace WiiFly.GUI {
    public class ModeWidgetController : MonoBehaviour {
        #region Fields
        private TMP_Text _text;
        private Image _image;
        #endregion

        #region Unity Methods
        protected void Awake()
        {
            _text = GetComponentInChildren<TMP_Text>();
            Image[] images = GetComponentsInChildren<Image>();
            _image = images[1];
        }
        #endregion

        #region Private Methods
        private void ChangeToOrbitMode()
        {
            _text.text = "Orbit Mode";
            _image.sprite = Resources.Load<Sprite>("Sprites/orbit");
        }

        private void ChangeToFlyMode()
        {
            _text.text = "Fly Mode";
            _image.sprite = Resources.Load<Sprite>("Sprites/fly");
        }
        #endregion
    }
}
