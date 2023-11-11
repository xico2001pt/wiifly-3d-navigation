﻿using UnityEngine;

namespace WiiFly.Camera.Mode {
    public interface ICameraMode {
        public void Initialize(UnityEngine.Camera camera);
        public void Update(Vector2 cursorPosition, float intensity);
        public string GetModeName();
    }
}