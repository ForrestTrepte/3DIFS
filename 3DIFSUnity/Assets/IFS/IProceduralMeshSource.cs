using UnityEngine;

namespace IFS
{
    public interface IProceduralMeshSource
    {
        Vector3[] Vertices { get; }
        int[] Indices { get;  }
    }
}
