using UnityEngine;
using UnityEngine.Assertions;

namespace IFS.Utilities
{
    public class HideOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            Renderer renderer = GetComponent<Renderer>();
            Assert.IsNotNull(renderer);
            renderer.enabled = false;
        }
    }
}