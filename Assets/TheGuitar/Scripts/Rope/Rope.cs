using System;
using UnityEngine;
using System.Collections.Generic;

namespace Sago
{
    [RequireComponent(typeof(LineRenderer))]
    public partial class Rope : MonoBehaviour
    {
        protected readonly List<RopeSegments> Segments = new();

        protected event Action<int> OnSimulationPasses;

        private Vector3 _startPosition;

        protected virtual void Awake()
        {
            if (!lineRenderer)
            {
#if UNITY_EDITOR

                Debug.LogWarning("No line renderer is attached to this game object.", this);
#endif
                return;
            }

            _startPosition = anchorA.position;

            lineRenderer.positionCount = ropeSettings.ropeLength;

            for (var i = 0; i < ropeSettings.ropeLength; i++)
            {
                Segments.Add(new RopeSegments(_startPosition));

                _startPosition.y -= ropeSettings.segmentSize;
            }
        }

        private void Update()
        {
            Draw();
        }

        private void FixedUpdate()
        {
            PhysicSimulation();

            for (var i = 0; i < physicSettings.simulationPasses; i++)
            {
                OnSimulationPasses?.Invoke(i);

                SimulationPasses();
            }
        }

        private void Draw()
        {
            var ropePositions = new Vector3[ropeSettings.ropeLength];

            for (var i = 0; i < Segments.Count; i++)
            {
                ropePositions[i] = Segments[i].CurrentPosition;
            }

            lineRenderer.SetPositions(ropePositions);
        }

        private void PhysicSimulation()
        {
            for (var i = 0; i < Segments.Count; i++)
            {
                var segment = Segments[i];

                var velocity = (segment.CurrentPosition - segment.PreviousPosition) * physicSettings.damping;

                segment.PreviousPosition = segment.CurrentPosition;

                segment.CurrentPosition += velocity;

                Segments[i] = segment;
            }
        }

        private void SimulationPasses()
        {
            var firstSegment = Segments[0];

            firstSegment.CurrentPosition = anchorA.position;

            Segments[0] = firstSegment;

            var lastSegment = Segments[^1];

            lastSegment.CurrentPosition = anchorB.position;

            Segments[^1] = lastSegment;

            for (var i = 0; i < ropeSettings.ropeLength - 1; i++)
            {
                var currentSegment = Segments[i];

                var nextSegment = Segments[i + 1];

                var distance = (currentSegment.CurrentPosition - nextSegment.CurrentPosition).magnitude;

                var difference = distance - ropeSettings.segmentSize;

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

                Segments[i] = currentSegment;

                Segments[i + 1] = nextSegment;
            }
        }

        private void OnDestroy()
        {
            OnSimulationPasses = null;
        }
    }
}
