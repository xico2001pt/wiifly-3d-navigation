using UnityEngine;

namespace WiiFly.Cursor {
    public struct CursorData {
        /** The x position ranges from -1 to 1, where -1 is the left side of the screen and 1 is the right side.
         * The y position ranges from -1 to 1, where -1 is the top of the screen and 1 is the bottom.
         */
        public Vector2 Position;
        public float Intensity;
        
        /**
         * Normalizes the given value to a value between 0 and 1.
         * @param value The value to normalize.
         * @param min The minimum value of the range.
         * @param max The maximum value of the range.
         * @return The normalized value.
         */
        public static float GetNormalizedValue(float value, float min, float max) {
            return (value - min) / (max - min);
        }
        
        public Vector2 GetNormalizedPosition() {
            return new Vector2(
                GetNormalizedValue(Position.x, -1, 1),
                GetNormalizedValue(Position.y, -1, 1)
            );
        }
    }
}
