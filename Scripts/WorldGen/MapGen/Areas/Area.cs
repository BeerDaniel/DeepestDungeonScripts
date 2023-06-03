using UnityEngine;
using UnityEngine.AI;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Abstract Class for Areas
    /// </summary>
    public abstract class Area
    {       
        public Facet facet;
        public GameObject gameObject;

        protected MapGenDataSet _mGDS;

        /// <summary>
        /// Initalization Method of all Area Childs
        /// </summary>
        /// <param name="facet">Facet of Area</param>
        /// <param name="heightManipulator">HeightManipulator Instance</param>
        /// <param name="mGDS">WorldGen DataObject</param>
        public void Init(Facet facet, MapGenDataSet mGDS)
        {
            this.facet = facet;
            _mGDS = mGDS;
            gameObject = BuildAreaObject();
            SetName(gameObject);
            SetLayer(gameObject);
            NavMeshModifier mod = gameObject.AddComponent<NavMeshModifier>();
            mod.name = "Park";
            mod.overrideArea = true;
            mod.area = 5;
        }

        /// <summary>
        /// Set Name of Area
        /// </summary>
        /// <param name="gameObject">Object to set Name</param>
        private void SetName(GameObject gameObject)
        {
            gameObject.name = SetAreaName();            
        }

        /// <summary>
        /// Set Layer of Area
        /// </summary>
        /// <param name="gameObject">Object to set Layer</param>
        private void SetLayer(GameObject gameObject)
        {
            gameObject.layer = SetAreaLayer();
        }

        /// <summary>
        /// Used to Implement Generation Behaviour
        /// </summary>
        /// <returns>Generated GameObject</returns>
        public abstract GameObject BuildAreaObject();

        /// <summary>
        /// Force Child to set a name
        /// </summary>
        /// <returns>ObjectName</returns>
        protected abstract string SetAreaName();

        /// <summary>
        /// Force Child to set a Layer
        /// </summary>
        /// <returns>ObjectLayer</returns>
        protected abstract int SetAreaLayer();
    }
}

