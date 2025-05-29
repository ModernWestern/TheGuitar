using UnityEngine;
using System.Collections.Generic;

namespace Sago
{
    public class GameInstance : MonoBehaviour
    {
        [SerializeField]
        private List<LifecycleScriptableObject> initializables;

        private void Awake()
        {
            foreach (var component in initializables)
            {
                component.Initialize();
            }
        }

        private void OnDestroy()
        {
            foreach (var component in initializables)
            {
                component.Deinitialize();
            }
        }
    }
}
