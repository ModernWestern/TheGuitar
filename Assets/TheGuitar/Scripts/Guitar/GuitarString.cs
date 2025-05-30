using UnityEngine;
using UnityEngine.Events;

namespace Sago.Guitar
{
    public class GuitarString : Rope
    {
        [SerializeField]
        private LayerMask collisionMask;

        [SerializeField]
        private float collisionRadius = 0.1f;

        [SerializeField]
        private float friction = 0.1f;

        [SerializeField]
        private GuitarStringNote note;

        [Space, SerializeField]
        private UnityEvent<GuitarStringNote> onStringReleased;

        private bool _collisionExit;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!lineRenderer)
            {
                return;
            }

            var width = ropeSettings.ropeWidth;

            lineRenderer.widthCurve = new AnimationCurve(new Keyframe(width, width));
        }

#endif

        protected override void Awake()
        {
            base.Awake();

            OnSimulationPasses += pass =>
            {
                if (pass % 3 == 0)
                {
                    Collision();
                }
            };
        }

        private void Collision()
        {
            var isColliding = false;

            for (var i = 1; i < Segments.Count; i++)
            {
                var segment = Segments[i];

                var velocity = segment.CurrentPosition - segment.PreviousPosition;

                var colliders = Physics2D.OverlapCircleAll(segment.CurrentPosition, collisionRadius, collisionMask);

                foreach (var col in colliders)
                {
                    var closestPoint = col.ClosestPoint(segment.CurrentPosition);

                    var distance = Vector2.Distance(segment.CurrentPosition, closestPoint);

                    if (distance < collisionRadius)
                    {
                        isColliding = true;

                        var normal = (segment.CurrentPosition - closestPoint).normalized;

                        if (normal == Vector2.zero)
                        {
                            normal = (segment.CurrentPosition - (Vector2)col.transform.position).normalized;
                        }

                        var depth = collisionRadius - distance;

                        segment.CurrentPosition += normal * depth;

                        velocity = Vector2.Reflect(velocity, normal) * friction;
                    }
                }

                segment.PreviousPosition = segment.CurrentPosition - velocity;

                Segments[i] = segment;
            }

            if (_collisionExit && !isColliding)
            {
                onStringReleased?.Invoke(note);
            }

            _collisionExit = isColliding;
        }
    }
}
