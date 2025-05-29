using System;
using UnityEngine;

namespace Sago
{
    public partial class Rope
    {
        [Serializable]
        public class RopeSettings
        {
            public int ropeLength;
            public float ropeWidth;
            [Range(0, 1)] public float segmentSize = 0.1f;
        }

        [Serializable]
        public class PhysicSettings
        {
            [Range(0, 1)] public float damping = 1;
            public int simulationPasses = 50;
        }
        
        [SerializeField]
        private Transform anchorA, anchorB;

        [SerializeField]
        protected LineRenderer lineRenderer;

        [SerializeField]
        protected RopeSettings ropeSettings;

        [SerializeField]
        protected PhysicSettings physicSettings;
    }
}
