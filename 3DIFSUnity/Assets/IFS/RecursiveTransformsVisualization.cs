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
        private float previousLevelOfRecursion;

        private void Start()
        {
            definition = GetComponentInParent<IfsDefinitionBehaviour>();
            Assert.IsNotNull(definition);
            Debug.Assert(definition != null);
        }

        private void Update()
        {
            if (previousLevelOfRecursion != LevelOfRecursion)
            {
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                Visualize(definition?.Definition);
                previousLevelOfRecursion = LevelOfRecursion;
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
            VisualizeToMeshRecursive(Matrix4x4.identity, 0, transforms, Matrix4x4.identity);
            pm.ToMesh(mesh);
        }

        private void VisualizeToMeshRecursive(Matrix4x4 transform, int level, Matrix4x4[] transforms, Matrix4x4 parentTransform)
        {
            if (level >= LevelOfRecursionInt - 1)
            {
                // Leaf at final level of recursion. Output with this transform.
                if (LevelOfRecursion == LevelOfRecursionInt)
                    pm.AddTransformed(cube, transform);
                else
                    pm.AddTransformed(cube, MatrixTrsLerp(parentTransform, transform, 1.0f + LevelOfRecursion - LevelOfRecursionInt));
                return;
            }

            for (int i = 0; i < transforms.Length; ++i)
            {
                Matrix4x4 childTransform = transforms[i] * transform;
                VisualizeToMeshRecursive(childTransform, level + 1, transforms, transform);
            }
        }

        private Matrix4x4 MatrixTrsLerp(Matrix4x4 a, Matrix4x4 b, float t)
        {
            // $TODO: Test that this works correctly with rotations and ordered combinations of all three transforms. Unit test?
            GetMatrixTrs(a, out Vector3 aT, out Quaternion aR, out Vector3 aS);
            GetMatrixTrs(b, out Vector3 bT, out Quaternion bR, out Vector3 bS);
            Matrix4x4 result = Matrix4x4.TRS(
                    Vector3.Lerp(aT, bT, t),
                    Quaternion.Slerp(aR, bR, t),
                    Vector3.Lerp(aS, bS, t));
            return result;
        }

        private void GetMatrixTrs(Matrix4x4 matrix, out Vector3 translation, out Quaternion rotation, out Vector3 scale)
        {
            translation = matrix.GetColumn(3);
            rotation = Quaternion.LookRotation(matrix.GetColumn(2), matrix.GetColumn(1));
            scale = new Vector3(matrix.GetColumn(0).magnitude, matrix.GetColumn(1).magnitude, matrix.GetColumn(2).magnitude);
        }
    }
}
