using UnityEngine;

namespace Sago
{
    public struct RopeSegments
    {
        public Vector2 CurrentPosition { get; set; }
        public Vector2 PreviousPosition { get; set; }

        public RopeSegments(Vector2 position)
        {
            CurrentPosition = position;
            PreviousPosition = position;
        }
    }
}
