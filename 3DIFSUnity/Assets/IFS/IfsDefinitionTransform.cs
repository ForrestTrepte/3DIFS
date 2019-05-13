using System;
using System.Collections.Generic;
using UnityEngine;

namespace IFS
{
    [Serializable]
    public class IfsTransform
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;

        public void ApplyToUnityTransform(Transform unityTransform)
        {
            unityTransform.localPosition = Position;
            unityTransform.localEulerAngles = Rotation;
            unityTransform.localScale = Scale;
        }

        public Matrix4x4 GetMatrix()
        {
            return Matrix4x4.TRS(Position, Quaternion.Euler(Rotation), Scale);
        }
    }
}
