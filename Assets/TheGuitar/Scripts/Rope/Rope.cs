using System;
using UnityEngine;
using System.Collections.Generic;

namespace Sago
{
    using Physics;

    [RequireComponent(typeof(LineRenderer))]
    public class Rope : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            public int ropeLength;
            public float ropeWidth;
            [Range(0, 1)] public float segmentSize = 0.1f;
            [Range(0, 1)] public float damping = 1;
            public int simulationPasses = 50;
        }

        [SerializeField]
        private LineRenderer lineRenderer;

        [SerializeField]
        private Settings settings;

        [SerializeField]
        private Transform anchorA, anchorB;

        private readonly List<RopeSegments> _segments = new();
        private Vector3 _startPosition;

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (!lineRenderer)
            {
                return;
            }

            var width = settings.ropeWidth;

            lineRenderer.widthCurve = new AnimationCurve(new Keyframe(width, width));
        }

#endif
        private void Awake()
        {
            if (!lineRenderer)
            {
#if UNITY_EDITOR

                Debug.LogWarning("No line renderer is attached to this game object.", this);
#endif
                return;
            }

            _startPosition = anchorA.position;

            lineRenderer.positionCount = settings.ropeLength;

            for (var i = 0; i < settings.ropeLength; i++)
            {
                _segments.Add(new RopeSegments(_startPosition));

                _startPosition.y -= settings.segmentSize;
            }
        }

        public void Update()
        {
            Draw();
        }

        private void FixedUpdate()
        {
            PhysicSimulation();

            for (var i = 0; i < settings.simulationPasses; i++)
            {
                SimulationPasses();
            }
        }

        private void Draw()
        {
            var ropePositions = new Vector3[settings.ropeLength];

            for (var i = 0; i < _segments.Count; i++)
            {
                ropePositions[i] = _segments[i].CurrentPosition;
            }

            lineRenderer.SetPositions(ropePositions);
        }

        private void PhysicSimulation()
        {
            for (var i = 0; i < _segments.Count; i++)
            {
                var segment = _segments[i];

                var velocity = (segment.CurrentPosition - segment.PreviousPosition) * settings.damping;

                segment.PreviousPosition = segment.CurrentPosition;
                segment.CurrentPosition += velocity;
                segment.CurrentPosition += PhysicData.Gravity * Time.fixedDeltaTime;

                _segments[i] = segment;
            }
        }

        private void SimulationPasses()
        {
            var firstSegment = _segments[0];
            firstSegment.CurrentPosition = anchorA.position;
            _segments[0] = firstSegment;

            var lastSegment = _segments[^1];
            lastSegment.CurrentPosition = anchorB.position;
            _segments[^1] = lastSegment;

            for (var i = 0; i < settings.ropeLength - 1; i++)
            {
                var currentSegment = _segments[i];

                var nextSegment = _segments[i + 1];

                var distance = (currentSegment.CurrentPosition - nextSegment.CurrentPosition).magnitude;

                var difference = distance - settings.segmentSize;

                var changeDirection = (currentSegment.CurrentPosition - nextSegment.CurrentPosition).normalized;

                var changeVector = changeDirection * difference;

                if (i != 0)
                {
                    currentSegment.CurrentPosition -= changeVector * 0.5f;
                    nextSegment.CurrentPosition += changeVector * 0.5f;
                }
                else
                {
                    nextSegment.CurrentPosition += changeVector;
                }

                _segments[i] = currentSegment;
                _segments[i + 1] = nextSegment;
            }
        }
    }
}
