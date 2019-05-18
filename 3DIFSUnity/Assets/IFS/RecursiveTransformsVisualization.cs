using UnityEngine;
using UnityEngine.Assertions;

namespace IFS
{
    public class RecursiveTransformsVisualization : MonoBehaviour
    {
        public float LevelOfRecursion;

        private IfsDefinitionBehaviour definition;
        private CubeMeshSource cube = new CubeMeshSource();
        private ProceduralMesh pm = new ProceduralMesh();
        private bool needsUpdate = true;

        private void Start()
        {
            definition = GetComponentInParent<IfsDefinitionBehaviour>();
            Assert.IsNotNull(definition);
            Debug.Assert(definition != null);
        }

        private void Update()
        {
            if (needsUpdate)
            {
                Visualize(definition?.Definition);
                needsUpdate = false;
            }
        }

        public void OnDefinitionChanged()
        {
            if (!isActiveAndEnabled)
                return;

            needsUpdate = true;
        }

        private void Visualize(IfsDefinition definition)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            if (meshFilter.mesh == null)
            {
                meshFilter.mesh = new Mesh();
            }
            VisualizeToMesh(meshFilter.mesh);
        }

        private void VisualizeToMesh(Mesh mesh)
        {
            cube.Init();
            pm.Reset();
            pm.AddAtPosition(cube, Vector3.zero);
            pm.ToMesh(mesh);
        }
    }
}
