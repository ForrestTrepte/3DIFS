using System;
using UnityEngine;

namespace IFS
{
    public class PointCloudMesh
    {
        private PointCloudPolygonMesh polygon = new PointCloudPolygonMesh();
        private Vector3[] vertices;
        private int vertexIndex;
        private int[] indices;
        private int indexIndex;

        public void Init(int numberOfPoints, int pointPolygonNumberOfSides, float pointPolygonDiameter, Vector3 right, Vector3 up)
        {
            polygon.Init(pointPolygonNumberOfSides, pointPolygonDiameter, right, up);
            InitVertices(numberOfPoints);
        }

        private void InitVertices(int numberOfPoints)
        {
            int vertexCount = numberOfPoints * polygon.Vertices.Length;
            if (vertices == null || vertices.Length != vertexCount)
            {
                vertices = new Vector3[vertexCount];
            }
            vertexIndex = 0;

            int indexCount = numberOfPoints * polygon.Indices.Length;
            if (indices == null || indices.Length != indexCount)
            {
                indices = new int[indexCount];
            }
            indexIndex = 0;
        }

        public void Add(Vector3 p)
        {
            int polygonIndicesLength = polygon.Indices.Length;
            for (int i = 0; i < polygonIndicesLength; ++i)
            {
                indices[indexIndex++] = vertexIndex + polygon.Indices[i];
            }

            int polygonVerticesLength = polygon.Vertices.Length;
            for (int i = 0; i < polygonVerticesLength; ++i)
            {
                vertices[vertexIndex++] = polygon.Vertices[i] + p;
            }
        }

        public Mesh ToMesh()
        {
            Debug.Assert(vertexIndex == vertices.Length);
            Debug.Assert(indexIndex == indices.Length);
            Mesh mesh = new Mesh();
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            mesh.vertices = vertices;
            mesh.triangles = indices;
            return mesh;
        }
    }
}
