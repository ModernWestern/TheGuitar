using System;
using UnityEngine;

namespace Sago.Physics
{
    [CreateAssetMenu(fileName = "PhysicData", menuName = "Sago/Scriptable Objects/PhysicData")]
    public class PhysicData : LifecycleScriptableObject
    {
        [SerializeField]
        private Vector2 gravity = new(0f, -9.8f);

        private static Func<Vector3> _getGravity;

        public override void Initialize()
        {
            _getGravity += () => gravity;
        }

        public override void Deinitialize()
        {
            _getGravity = null;
        }

        public static Vector2 Gravity => _getGravity?.Invoke() ?? Vector2.up * -9.8f;
    }
}
