﻿using UnityEngine;
using UnityEngine.Assertions;

namespace IFS
{
    public class RandomTransformsVisualization : MonoBehaviour
    {
        public int RandomSeed = 12345;
        public int NumberOfWarmUpPoints = 1000;
        public int NumberOfRenderedPoints = 10000;
        public int PointPolygonNumberOfSides = 3;
        public float PointPolygonDiameter = 0.01f;
        [Tooltip("When the camera moves by more than this angle (in degrees) the point cloud splats will be recalculated.")]
        public float CameraAngleUpdateDelta = 5.0f;

        private IfsDefinitionBehaviour definition;
        private PointCloudPolygonMeshSource polygon = new PointCloudPolygonMeshSource();
        private ProceduralMesh pm = new ProceduralMesh();
        private bool needsUpdate = true;
        private Vector3 cameraUp;
        private Vector3 cameraRight;

        private void Start()
        {
            definition = GetComponentInParent<IfsDefinitionBehaviour>();
            Assert.IsNotNull(definition);
            Debug.Assert(definition != null);
        }

        private void Update()
        {
            if (CheckCameraAngle())
            {
                needsUpdate = true;
            }

            if (needsUpdate)
            {
                SetCameraAngle();
                Visualize(definition?.Definition);
                needsUpdate = false;
            }
        }

        private bool CheckCameraAngle()
        {
            Vector3 currentCameraRight = Camera.main.transform.right;
            float rightDelta = Vector3.Angle(cameraRight, currentCameraRight);
            if (rightDelta > CameraAngleUpdateDelta)
                return true;

            Vector3 currentCameraUp = Camera.main.transform.up;
            float upDelta = Vector3.Angle(cameraUp, currentCameraUp);
            if (upDelta > CameraAngleUpdateDelta)
                return true;

            return false;
        }

        private void SetCameraAngle()
        {
            cameraRight = Camera.main.transform.right;
            cameraUp = Camera.main.transform.up;
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
            Random.InitState(RandomSeed);
            Matrix4x4[] transforms = definition.Definition.GetMatrices();
            polygon.Init(PointPolygonNumberOfSides, PointPolygonDiameter, cameraRight, cameraUp);
            pm.Reset();

            Vector4 p = new Vector4(0, 0, 0, 1);
            for (int i = 0; i < NumberOfWarmUpPoints + NumberOfRenderedPoints; ++i)
            {
                // TODO: Weight probability of each transform by determinant or technique from https://math.stackexchange.com/questions/1900337/how-to-optimally-adjust-the-probabilities-for-the-random-ifs-algorithm.
                int transformIndex = Random.Range(0, transforms.Length);
                p = transforms[transformIndex] * p;
                if (i >= NumberOfWarmUpPoints)
                {
                    pm.AddAtPosition(polygon, p);
                }
            }

            pm.ToMesh(mesh);
        }
    }
}
