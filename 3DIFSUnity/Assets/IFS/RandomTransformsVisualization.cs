using UnityEngine;
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

        private IfsDefinitionBehaviour definition;
        private PointCloudMesh pcm = new PointCloudMesh();

        private void Start()
        {
            definition = GetComponentInParent<IfsDefinitionBehaviour>();
            Assert.IsNotNull(definition);
            Debug.Assert(definition != null);

            OnDefinitionChanged();
        }

        public void OnDefinitionChanged()
        {
            Visualize(definition?.Definition);
        }

        private void Visualize(IfsDefinition definition)
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            if (meshFilter == null)
            {
                meshFilter = gameObject.AddComponent<MeshFilter>();
            }
            meshFilter.mesh = VisualizeToMesh();
        }

        private Mesh VisualizeToMesh()
        {
            Random.InitState(RandomSeed);
            Matrix4x4[] transforms = definition.Definition.GetMatrices();
            // TODO: Update in response to camera movement.
            pcm.Init(NumberOfRenderedPoints, PointPolygonNumberOfSides, PointPolygonDiameter, -Camera.main.transform.right, Camera.main.transform.up);
            Vector4 p = new Vector4(0, 0, 0, 1);
            for (int i = 0; i < NumberOfWarmUpPoints + NumberOfRenderedPoints; ++i)
            {
                // TODO: Weight probability of each transform by determinant or technique from https://math.stackexchange.com/questions/1900337/how-to-optimally-adjust-the-probabilities-for-the-random-ifs-algorithm.
                int transformIndex = Random.Range(0, transforms.Length);
                p = transforms[transformIndex] * p;
                if (i >= NumberOfWarmUpPoints)
                {
                    pcm.Add(p);
                }
            }

            return pcm.ToMesh();
        }
    }
}
