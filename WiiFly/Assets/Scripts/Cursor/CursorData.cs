using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WiiFly.Cursor
{
    public struct CursorData
    {
        public float x;
        public float y;

        public CursorData(float x, float y)
        {
            this.x = x; 
            this.y = y;
        }
    }
}
