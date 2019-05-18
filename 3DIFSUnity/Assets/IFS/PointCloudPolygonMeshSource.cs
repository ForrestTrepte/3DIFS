using System;
using UnityEngine;

namespace IFS
{
    public class PointCloudPolygonMeshSource : IProceduralMeshSource
    {
        public Vector3[] Vertices { get; private set; }
        public int[] Indices { get; private set; }

        public void Init(int numberOfSides, float diameter, Vector3 right, Vector3 up)
        {
            int vertexCount = numberOfSides;

            if (Vertices == null || Vertices.Length != vertexCount)
            {
                Vertices = new Vector3[vertexCount];
            }

            for (int i = 0; i < numberOfSides; ++i)
            {
                Vertices[i] = PolygonPoint(i, numberOfSides, diameter, right, up);
            }

            int indexCount = (numberOfSides - 2) * 3;
            if (Indices == null || Indices.Length != indexCount)
            {
                Indices = new int[indexCount];
            }

            for (int i = 0; i < numberOfSides - 2; ++i)
            {
                Indices[i * 3 + 0] = 0;
                Indices[i * 3 + 1] = i + 1;
                Indices[i * 3 + 2] = i + 2;
            }
        }

        private Vector3 PolygonPoint(int index, int pointPolygonNumberOfSides, float pointPolygonDiameter, Vector3 right, Vector3 up)
        {
            // Negative index for clockwise winding order.
            float angle = Mathf.PI * 2 * -index / pointPolygonNumberOfSides;
            Vector3 point = right * Mathf.Cos(angle) * pointPolygonDiameter + up * Mathf.Sin(angle) * pointPolygonDiameter;
            return point;
        }
    }
}
