using UnityEngine;
using WatStudios.DeepestDungeon.HelperClasses;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for dividing Mesh Triangles
    /// </summary>
    public static class TrisManipulator
    {
        /// <summary>
        /// Divides Triangles of Object Mesh
        /// </summary>
        /// <param name="gameObject">Object with Mesh to divide/Object with Mesh(es) in Children to divide</param>
        /// <param name="smoothnessLevel">Level of Division (approximated: only 1->4 and 1->9 divisions possible)</param>
        public static void SmoothObject(GameObject gameObject, int smoothnessLevel)
        {
            MeshFilter[] filters = gameObject.GetComponentsInChildren<MeshFilter>();

            //Get Meshes
            foreach (MeshFilter meshFilter in filters)
            {
                Mesh mesh = meshFilter.mesh;


                if (mesh != null)
                {
                    //Subdivide
                    MeshHelper.Subdivide(mesh, smoothnessLevel);

                    mesh.RecalculateNormals();
                    mesh.RecalculateBounds();
                    mesh.RecalculateTangents();

                }
            }
        }
    }
}
