using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace IFS
{
    public class WireframeCubesVisualization : MonoBehaviour
    {
        public WireframeCubeVisualization CubePrefab;

        private IfsDefinitionBehaviour definition;
        private WireframeCubeVisualization mainCube;
        private List<WireframeCubeVisualization> transformedCubes = new List<WireframeCubeVisualization>();

        private void Start()
        {
            definition = GetComponentInParent<IfsDefinitionBehaviour>();
            Assert.IsNotNull(definition);
            Debug.Assert(definition != null);

            OnDefinitionChanged();
        }

        private void OnValidate()
        {
            Assert.IsNotNull(CubePrefab);
        }

        public void OnDefinitionChanged()
        {
            if (!isActiveAndEnabled)
                return;

            Visualize(definition?.Definition);
        }

        private void Visualize(IfsDefinition definition)
        {
            if (mainCube == null)
            {
                mainCube = Instantiate<WireframeCubeVisualization>(CubePrefab, transform);
                mainCube.name = "main";
            }

            ClearTransformedCubes();

            if (definition == null)
                return;

            for (int i = 0; i < definition.Transforms.Count; ++i)
            {
                IfsTransform ifsTransform = definition.Transforms[i];
                WireframeCubeVisualization transformedCube = Instantiate<WireframeCubeVisualization>(CubePrefab, transform);
                transformedCube.name = $"t{i}";
                ifsTransform.ApplyToUnityTransform(transformedCube.transform);
                transformedCubes.Add(transformedCube);
            }
        }

        private void ClearTransformedCubes()
        {
            foreach (WireframeCubeVisualization transformedCube in transformedCubes)
            {
                Destroy(transformedCube.gameObject);
            }
            transformedCubes.Clear();
        }
    }
}
