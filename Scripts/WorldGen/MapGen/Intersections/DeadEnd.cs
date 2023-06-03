using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for Generating a Circle Like Dead End with a Single Bridge
    /// </summary>
    public class DeadEnd : Intersection
    {

        protected override string SetIntersectionName()
        {
            return "DeadEnd";
        }

        protected override int SetIntersectionLayer()
        {
            return LayerMask.NameToLayer("Floor");
        }

        public override GameObject BuildIntersectionObject()
        {
            List<Vector3> corners = new List<Vector3>();

            for (int i = 0; i < 18; i++)
            {
                Vector2 vec1 = (corner.IncidentHalfEdge.TwinHalfEdge.StartCorner.position - corner.position).normalized;
                Vector2 vec2 = Quaternion.AngleAxis(-(20 * i), Vector3.forward) * vec1;


                Vector2 point = corner.position + (vec2 * _mGDS.BridgeWidth); //Platzhalter
                corners.Add(new Vector3(point.x, 0, point.y));
            }

            GameObject deadEnd = new GameObject(name);
            MeshFilter filter = deadEnd.AddComponent<MeshFilter>();
            ProBuilderMesh mesh = deadEnd.AddComponent<ProBuilderMesh>();

            float houseHeight = _mGDS.TerrainThicknes; //Platzhalter
            try
            {
                mesh.CreateShapeFromPolygon(corners, houseHeight, false);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DD_Team: ProBuilder doesn't load the assets correctly: " + e);
            }
            deadEnd.GetComponent<MeshRenderer>().materials = new Material[] { Resources.Load<Material>("WorldGen/Materials/Asphalt_material_14") };

            TrisManipulator.SmoothObject(deadEnd, _mGDS.Smoothness);
            HeightManipulator.ManipulateMeshHeightToNoise(deadEnd);

            deadEnd.AddComponent<MeshCollider>(); //Add Collider at last so it is automatically modified

            return deadEnd;
        }
    }
}
