using UnityEngine;
using UnityEngine.AI;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Abstract Class for Intersections
    /// </summary>
    public abstract class Intersection
    {
        public Corner corner;
        public string name;
        public GameObject gameObject;

        protected MapGenDataSet _mGDS;

        /// <summary>
        /// Initalization Method of all Intersection Childs
        /// </summary>
        /// <param name="corner">Corner representatig Intersection</param>
        /// <param name="heightManipulator">HeightManipulator Instance</param>
        /// <param name="mGDS">WorldGen DataObject</param>
        public void Init(Corner corner, MapGenDataSet mGDS)
        {
            this.corner = corner;
            _mGDS = mGDS;
            gameObject = BuildIntersectionObject();
            //gameObject.transform.position += new Vector3(0, -0.01f, 0); //Prevent z-Fighting
            SetName(gameObject);
            SetLayer(gameObject);
            NavMeshModifier mod = gameObject.AddComponent<NavMeshModifier>();
            mod.name = "Intersection";
            mod.overrideArea = true;
            mod.area = 6;
        }

        /// <summary>
        /// Set Name of Intersection
        /// </summary>
        /// <param name="gameObject">Object to set Name</param>
        private void SetName(GameObject gameObject)
        {
            name = SetIntersectionName();
            gameObject.name = name;
        }

        /// <summary>
        /// Set Layer of Intersection
        /// </summary>
        /// <param name="gameObject">Object to set Layer</param>
        private void SetLayer(GameObject gameObject)
        {
            gameObject.layer = SetIntersectionLayer();
        }

        /// <summary>
        /// Used to Implement Generation Behaviour
        /// </summary>
        /// <returns>Generated GameObject</returns>
        public abstract GameObject BuildIntersectionObject();

        /// <summary>
        /// Force Child to set a name
        /// </summary>
        /// <returns>ObjectName</returns>
        protected abstract string SetIntersectionName();

        /// <summary>
        /// Force Child to set a Layer
        /// </summary>
        /// <returns>ObjectLayer</returns>
        protected abstract int SetIntersectionLayer();
    }
}