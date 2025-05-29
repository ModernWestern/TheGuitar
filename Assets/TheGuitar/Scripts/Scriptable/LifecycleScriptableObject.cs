using System;
using UnityEngine;

namespace Sago
{
    [Serializable]
    public abstract class LifecycleScriptableObject : ScriptableObject
    {
        public abstract void Initialize();

        public abstract void Deinitialize();
    }
}
