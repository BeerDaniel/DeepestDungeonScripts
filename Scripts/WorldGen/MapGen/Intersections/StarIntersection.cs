using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for Creating a Starlike Intersection based on Bridge count
    /// </summary>
    public class StarIntersection : Intersection
    {
        protected override string SetIntersectionName()
        {
            return "StarIntersection";
        }

        protected override int SetIntersectionLayer()
        {
            return LayerMask.NameToLayer("Floor");
        }

        public override GameObject BuildIntersectionObject()
        {
            HalfEdge currentEdge = corner.IncidentHalfEdge;

            List<Vector3> intersectionCorners = new List<Vector3>();


            do
            {
                Vector2 vec1 = (currentEdge.Successor.StartCorner.position - currentEdge.StartCorner.position).normalized;
                Vector2 midpoint = currentEdge.StartCorner.position + (vec1 * 10f); //Platzhalter
                                                                                    //p1
                Vector2 p1 = midpoint + (Vector2)(Quaternion.AngleAxis(-(90), Vector3.forward) * vec1) * _mGDS.BridgeWidth / 2f;
                //p2
                Vector2 p2 = midpoint + (Vector2)(Quaternion.AngleAxis((90), Vector3.forward) * vec1) * _mGDS.BridgeWidth / 2f;
                //wh
                Vector2 vec2 = (currentEdge.Predecessor.StartCorner.position - currentEdge.StartCorner.position).normalized;

                //Get length of Bisection
                float a = _mGDS.BridgeWidth / 2f;
                float alphaDeg = Vector2.Angle(vec1, vec2) / 2f;
                float alphaRad = alphaDeg * Mathf.PI / 180;
                float b = a / Mathf.Tan(alphaRad);
                float c = Mathf.Sqrt(Mathf.Pow(a, 2) + Mathf.Pow(b, 2));
                float vectorLength;


                if (Vector2.SignedAngle(vec1, vec2) < 0)
                {
                    vectorLength = -c;
                }
                else
                {
                    vectorLength = c;
                }

                Vector2 bisection = (vec1 + vec2).normalized * vectorLength;
                Vector2 p3 = currentEdge.StartCorner.position + bisection;
                //Add
                intersectionCorners.Add(new Vector3(p1.x, 0, p1.y));
                intersectionCorners.Add(new Vector3(p2.x, 0, p2.y));
                intersectionCorners.Add(new Vector3(p3.x, 0, p3.y));
                //next
                currentEdge = currentEdge.Predecessor.TwinHalfEdge;
            } while (currentEdge != corner.IncidentHalfEdge);

            GameObject intersection = new GameObject(name);
            MeshFilter filter = intersection.AddComponent<MeshFilter>();
            ProBuilderMesh mesh = intersection.AddComponent<ProBuilderMesh>();

            float houseHeight = _mGDS.TerrainThicknes;

            try
            {
                mesh.CreateShapeFromPolygon(intersectionCorners, houseHeight, false);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("DD_Team: ProBuilder doesn't load the assets correctly: " + e);
            }

            intersection.GetComponent<MeshRenderer>().materials = new Material[] { Resources.Load<Material>("WorldGen/Materials/Asphalt_material_14") };
            //mesh.SetMaterial(mesh.faces, _mGDS.houseMaterial);


            //TrisManipulator.SmoothObject(intersection, 0);
            HeightManipulator.ManipulateMeshHeightToNoise(intersection);

            intersection.AddComponent<MeshCollider>(); //Add Collider at last so it is automatically modified

            return intersection;
        }
    }
}
