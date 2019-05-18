using System;
using UnityEngine;

namespace IFS
{
    public class CubeMeshSource : IProceduralMeshSource
    {
        public Vector3[] Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public void Init()
        {
            int vertexCount = 8;

            if (Vertices == null || Vertices.Length != vertexCount)
            {
                Vertices = new Vector3[vertexCount];
            }

            for (int x = 0; x < 2; ++x)
            {
                for (int y = 0; y < 2; ++y)
                {
                    for (int z = 0; z < 2; ++z)
                    {
                        Vector3 v = new Vector3(
                                x == 0 ? -0.5f : +0.5f,
                                y == 0 ? -0.5f : +0.5f,
                                z == 0 ? -0.5f : +0.5f);
                        Vertices[z * 4 + y * 2 + x] = v;
                    }
                }
            }

            int indexCount = 6 * 2 * 3; // each of 6 faces is two triangles
            if (Indices == null || Indices.Length != indexCount)
            {
                Indices = new int[indexCount];
            }

            SetFace(0, 0, 2, 3, 1);
            SetFace(1, 1, 3, 7, 5);
            SetFace(2, 5, 7, 6, 4);
            SetFace(3, 4, 6, 2, 0);
            SetFace(4, 2, 6, 7, 3);
            SetFace(5, 4, 0, 1, 5);
        }

        private void SetFace(int faceCount, int v0, int v1, int v2, int v3)
        {
            int i = faceCount * 6;
            Indices[i + 0] = v0;
            Indices[i + 1] = v1;
            Indices[i + 2] = v2;
            Indices[i + 3] = v0;
            Indices[i + 4] = v2;
            Indices[i + 5] = v3;
        }
    }
}
