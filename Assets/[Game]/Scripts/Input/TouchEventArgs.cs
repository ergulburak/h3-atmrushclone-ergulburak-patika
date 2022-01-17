using System;
using UnityEngine;

namespace _Game_.Scripts.Input
{
    public class TouchEventArgs : EventArgs
    {
        public TouchEventArgs(Vector2 position, float time)
        {
            Position = position;
            Time = time;
        }

        public Vector2 Position { get; }
        public float Time { get; }
    }
}
