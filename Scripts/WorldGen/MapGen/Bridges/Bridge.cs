using UnityEngine;
using UnityEngine.AI;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Abstract Class for Bridges
    /// </summary>
    public abstract class Bridge
    {
        public HalfEdge halfEdgeA;
        public HalfEdge halfEdgeB;
        public GameObject gameObject;

        protected MapGenDataSet _mGDS;

        /// <summary>
        /// Initalization Method of all Bridge Childs
        /// </summary>
        /// <param name="halfEdgeA">HalfEdge of Bridge</param>
        /// <param name="halfEdgeB">TwinHalfEdge of Bridge</param>
        /// <param name="heightManipulator">Height Manipulator Instance</param>
        /// <param name="mGDS">WorldGen DataObject</param>
        public void Init(HalfEdge halfEdgeA, HalfEdge halfEdgeB, MapGenDataSet mGDS)
        {
            this.halfEdgeA = halfEdgeA;
            this.halfEdgeB = halfEdgeB;
            _mGDS = mGDS;
            gameObject = BuildBridgeObject();
            SetName(gameObject);
            SetLayer(gameObject);
            NavMeshModifier mod = gameObject.AddComponent<NavMeshModifier>();
            mod.name = "Bridge";
            mod.overrideArea = true;
            mod.area = 4;
        }

        /// <summary>
        /// Set Name of Bridge
        /// </summary>
        private void SetName(GameObject gameObject)
        {
            gameObject.name = SetBridgeName();
            
        }

        /// <summary>
        /// Set Layer of Area
        /// </summary>
        /// <param name="gameObject">Object to set Layer</param>
        private void SetLayer(GameObject gameObject)
        {
            gameObject.layer = SetBridgeLayer();
        }

        /// <summary>
        /// Used to Implement Generation Behaviour
        /// </summary>
        /// <returns>Generated GameObject</returns>
        public abstract GameObject BuildBridgeObject();

        /// <summary>
        /// Force Child to set a name
        /// </summary>
        /// <returns>ObjectName</returns>
        protected abstract string SetBridgeName();

        /// <summary>
        /// Force Child to set a Layer
        /// </summary>
        /// <returns>ObjectLayer</returns>
        protected abstract int SetBridgeLayer();
    }
}