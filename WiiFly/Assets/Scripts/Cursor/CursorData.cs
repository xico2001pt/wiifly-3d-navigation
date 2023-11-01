using UnityEngine;

namespace WiiFly.Cursor {
    public struct CursorData {
        /** The x position ranges from 0 to 1, where 0 is the left side of the screen and 1 is the right side.
         * The y position ranges from 0 to 1, where 0 is the top of the screen and 1 is the bottom.
         */
        public Vector2 Position;
        public float intensity;
    }
}
