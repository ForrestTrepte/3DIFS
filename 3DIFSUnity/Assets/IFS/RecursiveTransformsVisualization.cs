using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace IFS
{
    public class RecursiveTransformsVisualization : MonoBehaviour
    {
        public float LevelOfRecursion;
        public int LevelOfRecursionInt { get { return Mathf.CeilToInt(LevelOfRecursion); } }

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
            Matrix4x4[] transforms = definition.Definition.GetMatrices();
            pm.Reset();
            VisualizeToMeshRecursive(Matrix4x4.identity, 0, transforms);
            pm.ToMesh(mesh);
        }

        private void VisualizeToMeshRecursive(Matrix4x4 transform, int level, Matrix4x4[] transforms)
        {
            if (level >= LevelOfRecursionInt - 1)
            {
                // Leaf at final level of recursion. Output with this transform.
                pm.AddTransformed(cube, transform);
                return;
            }

            for (int i = 0; i < transforms.Length; ++i)
            {
                Matrix4x4 childTransform = transforms[i] * transform;
                VisualizeToMeshRecursive(childTransform, level + 1, transforms);
            }
        }
    }
}
