using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for Generating a Single Bridge between two Corners
    /// </summary>
    public class SingleBridge : Bridge
    {
        protected override string SetBridgeName()
        {
            return "SingleBridge";
        }

        protected override int SetBridgeLayer()
        {
            return LayerMask.NameToLayer("Floor");
        }

        public override GameObject BuildBridgeObject()
        {
            Vector3 start = new Vector3(halfEdgeA.StartCorner.position.x, 0, halfEdgeA.StartCorner.position.y);
            Vector3 end = new Vector3(halfEdgeA.TwinHalfEdge.StartCorner.position.x, 0, halfEdgeA.TwinHalfEdge.StartCorner.position.y);

            float length = Vector3.Distance(start, end) - 9;//Platzhalter
            float width = _mGDS.BridgeWidth;

            GameObject bridge = Object.Instantiate(Resources.Load<GameObject>("WorldGen/Bridge"));

            bridge.transform.position = (start + end) / 2;
            bridge.transform.localScale = new Vector3(bridge.transform.localScale.x * width, bridge.transform.localScale.y * width, bridge.transform.localScale.z * length);
            bridge.transform.LookAt(end);



            TrisManipulator.SmoothObject(bridge, _mGDS.Smoothness);

            HeightManipulator.ManipulateMeshHeightToNoise(bridge);


            foreach (var meshFilter in bridge.GetComponentsInChildren<MeshFilter>())
            {
                meshFilter.gameObject.AddComponent<MeshCollider>();  //Add Collider at last so it is automatically modified           
            }

            return bridge;
        }
    }
}
