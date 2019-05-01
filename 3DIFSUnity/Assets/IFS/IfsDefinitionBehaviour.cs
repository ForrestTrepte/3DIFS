using System;
using UnityEngine;
using UnityEngine.Events;

namespace IFS
{
    public class IfsDefinitionBehaviour : MonoBehaviour
    {
        public IfsDefinition Definition;
        public UnityEvent OnDefinitionChanged;

        private bool wasDefinitionChanged = true;

        private void OnValidate()
        {
            wasDefinitionChanged = true;
        }

        private void Update()
        {
            if (wasDefinitionChanged)
            {
                OnDefinitionChanged.Invoke();
                wasDefinitionChanged = false;
            }
        }
    }
}
