using System.Collections.Generic;
using UnityEngine;

namespace IFS
{
    public class ProceduralMesh
    {
        private List<Vector3> vertices = new List<Vector3>();
        private List<int> indices = new List<int>();

        public void Reset()
        {
            vertices.Clear();
            indices.Clear();
        }

        public void AddAtPosition(IProceduralMeshSource source, Vector3 position)
        {
            int polygonIndicesLength = source.Indices.Length;
            for (int i = 0; i < polygonIndicesLength; ++i)
            {
                indices.Add(vertices.Count + source.Indices[i]);
            }

            int polygonVerticesLength = source.Vertices.Length;
            for (int i = 0; i < polygonVerticesLength; ++i)
            {
                vertices.Add(source.Vertices[i] + position);
            }
        }

        public void ToMesh(Mesh mesh)
        {
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = vertices.ToArray();
            mesh.triangles = indices.ToArray();
        }
    }
}
