using System;
using System.Collections.Generic;
using UnityEngine;

namespace IFS
{
    [Serializable]
    public class IfsDefinition
    {
        public List<IfsTransform> Transforms;

        public Matrix4x4[] GetMatrices()
        {
            Matrix4x4[] matrices = new Matrix4x4[Transforms.Count];
            for (int i = 0; i < Transforms.Count; ++i)
            {
                matrices[i] = Transforms[i].GetMatrix();
            }
            return matrices;
        }
    }
}