using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for Manipulating Meshes and Points to NoiseHeight
    /// </summary>
    public static class HeightManipulator
    {
        #region Private Fields

        private static float _offset;
        private static float _scale;
        private static float _mapSizeY;
        private static float _offsetY_Ground;

        #endregion

        /// <summary>
        /// Constructor for HeightManipulator
        /// </summary>
        /// <param name="noiseOffset">Offset for Noise Generation</param>
        /// <param name="noiseScale">Scale for Noise</param>
        /// <param name="mapSizeY">Map size Y</param>
        /// <param name="offsetY_Ground"> offsetY_Ground</param>
        public static void Init(float noiseOffset, float noiseScale, float mapSizeY,float offsetY_Ground)
        {
            _offset = noiseOffset;
            _scale = noiseScale;
            _mapSizeY = mapSizeY;
            _offsetY_Ground = offsetY_Ground;
        }

        #region Public Methods

        /// <summary>
        /// Manipulates every Mesh of the Gameobject or its Childs to Noise height (Vecor3(x,Noise,z))
        /// </summary>
        /// <param name="gameObject"></param>
        public static void ManipulateMeshHeightToNoise(GameObject gameObject)
        {
            //Get all Meshes
            MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>();            

            foreach (MeshFilter meshFilter in filters)
            {
                Mesh mesh = meshFilter.mesh;               

                if (mesh != null)
                {
                    Vector3[] vertices = mesh.vertices;

                    for (int i = 0; i < vertices.Length; i++)
                    {
                        Vector3 vertice = meshFilter.gameObject.transform.localToWorldMatrix.MultiplyPoint3x4(vertices[i]);
                        vertice.y = ManipulateYToNoise(vertice).y;
                        vertices[i] = meshFilter.gameObject.transform.worldToLocalMatrix.MultiplyPoint3x4(vertice);
                    }

                    mesh.vertices = vertices;
                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();
                    mesh.RecalculateTangents();
                }
            }
           

        }

        /// <summary>
        /// Adds Noiseheight to y Paramter of Point (Vector3(x,y+Noise,z))
        /// </summary>
        /// <param name="point">Point to Manipulate</param>
        /// <returns></returns>
        public static Vector3 ManipulateYToNoise(Vector3 point)
        {
            point.y += Mathf.PerlinNoise((point.x + _offset) / _scale, (point.z + _offset) / _scale) * _mapSizeY +_offsetY_Ground;
            return point;
        }

        /// <summary>
        /// Transforms Vector3 out of Vector2 with (Vector3(Vector2.x,Noise,Vector2.y))
        /// </summary>
        /// <param name="point">Point to Transform</param>
        /// <returns></returns>
        public static Vector3 TransformToVec3Noise(Vector2 point)
        {
            Vector3 point3 = new Vector3(point.x, 0, point.y);
            point3.y += Mathf.PerlinNoise((point3.x + _offset) / _scale, (point3.z + _offset) / _scale) * _mapSizeY +_offsetY_Ground;
            return point3;
        }

        #endregion
    }
}
