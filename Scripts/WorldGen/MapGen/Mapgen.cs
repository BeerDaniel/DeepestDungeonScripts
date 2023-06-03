using System.Collections.Generic;
using UnityEngine;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    public static class Mapgen
    {
        /// <summary>
        /// Creates new Map
        /// </summary>
        /// <param name="mGDS"></param>
        public static GameObject Create(MapGenDataSet mGDS)
        {
            InitSeedandHeightManipulator(mGDS);
            return CreateMap(mGDS);
        }

        /// <summary>
        /// Start Method for Gizmo Drawing
        /// </summary>
        public static void DrawGizmos(MapGenDataSet mGDS, bool mapPreview, bool createGhostTerrain)
        {
            InitSeedandHeightManipulator(mGDS);

            DrawLevelOutline(mGDS);
            DrawMapOutline(mGDS);

            if (mapPreview)
            {
                DrawMapPreview(mGDS);
            }

            if (createGhostTerrain)
            {
                DrawGhostTerrain(mGDS);
            }
        }

        #region Private Methods (Gizmos)

        /// <summary>
        /// Sets seed and Creates the HeightManipulator instance
        /// </summary>
        private static void InitSeedandHeightManipulator(MapGenDataSet mGDS)
        {
            Random.InitState(mGDS.Seed);

            //offset for Noise Generation (Random Noise is produced from random Offset)
            float offset = Random.Range(0, 1000);

            HeightManipulator.Init(offset, mGDS.NoiseScale, mGDS.MapSizeY, mGDS.OffsetY_Ground);
        }

        /// <summary>
        /// Gizmo for LevelOutline (Map+Borders)
        /// </summary>
        private static void DrawLevelOutline(MapGenDataSet mGDS)
        {
            Vector3 levelCenter = new Vector3(mGDS.MapSizeX / 2, (mGDS.MapSizeY + mGDS.OffsetY_Ceiling + mGDS.OffsetY_Ground) / 2, mGDS.MapSizeZ / 2);
            Vector3 levelSize = new Vector3(mGDS.MapSizeX + 2 * mGDS.BorderwidthXZ, mGDS.MapSizeY + mGDS.OffsetY_Ceiling + mGDS.OffsetY_Ground, mGDS.MapSizeZ + 2 * mGDS.BorderwidthXZ);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(levelCenter, levelSize);
        }

        /// <summary>
        /// Gizmo for MapOutline
        /// </summary>
        private static void DrawMapOutline(MapGenDataSet mGDS)
        {
            Vector3 mapCenter = new Vector3(mGDS.MapSizeX / 2, (mGDS.MapSizeY / 2) + mGDS.OffsetY_Ground, mGDS.MapSizeZ / 2);
            Vector3 mapSize = new Vector3(mGDS.MapSizeX, mGDS.MapSizeY, mGDS.MapSizeZ);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(mapCenter, mapSize);
        }

        /// <summary>
        /// Gizmos for Intersection and Bridge Preview
        /// </summary>
        private static void DrawMapPreview(MapGenDataSet mGDS)
        {
            RelativeNeighbourhoodGraph graph = new RelativeNeighbourhoodGraph(CreateGraphCorners(mGDS));

            DrawMapPreviewCorners(graph.corners);
            DrawMapPreviewEdges(graph.halfEdges);
            //DrawMapPreviewFacets(graph.facets); // Not useful cause too much clutter for Preview, also Gizmos have no depth buffer
        }

        /// <summary>
        /// Draw Preview Gizmos for Corners
        /// </summary>
        /// <param name="corners">List of Corners to Draw</param>
        private static void DrawMapPreviewCorners(List<Corner> corners)
        {

            foreach (var corner in corners)
            {
                //Draw Corners
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(HeightManipulator.TransformToVec3Noise(corner.position), 3);

                //Draw 2d representation of Corners
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(new Vector3(corner.position.x, 0, corner.position.y), 3);
            }

        }

        /// <summary>
        /// Draw Preview Gizmos for Corners
        /// </summary>
        /// <param name="halfEdges">List of Half Edges to Draw</param>
        private static void DrawMapPreviewEdges(List<HalfEdge> halfEdges)
        {
            //Remember already drawn Edges
            List<HalfEdge> alreadyVisited = new List<HalfEdge>();

            foreach (var halfEdge in halfEdges)
            {
                if (!alreadyVisited.Contains(halfEdge))
                {
                    //Draw Edges
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(HeightManipulator.TransformToVec3Noise(halfEdge.StartCorner.position), HeightManipulator.TransformToVec3Noise(halfEdge.TwinHalfEdge.StartCorner.position));

                    //Draw 2d representation of Edges
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(new Vector3(halfEdge.StartCorner.position.x, 0, halfEdge.StartCorner.position.y), new Vector3(halfEdge.TwinHalfEdge.StartCorner.position.x, 0, halfEdge.TwinHalfEdge.StartCorner.position.y));

                    alreadyVisited.Add(halfEdge);

                    //Add TwinHalfEdge to alredy visited, so every Bridge only creates one Gizmo instead of two
                    alreadyVisited.Add(halfEdge.TwinHalfEdge);
                }
            }
        }

        // Not useful cause too much clutter for Preview, also Gizmos have no depth buffer
        //private void DrawMapPreviewFacets(List<Facet> facets)
        //{
        //    foreach (var facet in facets)
        //    {
        //        HalfEdge currentEdge;

        //        bool flipFaces;

        //        if (facet.IsOutline)
        //        {
        //            flipFaces = true;
        //            Gizmos.color = Color.red;
        //        }
        //        else
        //        {
        //            flipFaces = false;
        //            Gizmos.color = Color.grey;
        //        }

        //        List<Vector3> corners = new List<Vector3>();
        //        currentEdge = facet.IncidentHalfEdge;

        //        do
        //        {


        //            if (currentEdge.Predecessor == currentEdge.TwinHalfEdge)
        //            {
        //                float vectorLength = mGDS.StreetSpacing;

        //                for (int i = 0; i < 7; i++)
        //                {
        //                    Vector2 vec1 = (currentEdge.Predecessor.StartCorner.position - currentEdge.StartCorner.position).normalized;
        //                    Vector2 vec2 = Quaternion.AngleAxis(-(90 + 30 * i), Vector3.forward) * vec1;

        //                    Vector2 bisectingOffset = vec2 * vectorLength;
        //                    Vector2 corner = currentEdge.StartCorner.position + bisectingOffset;
        //                    corners.Add(new Vector3(corner.x, 0, corner.y));
        //                }
        //            }
        //            else
        //            {
        //                float vectorLength = Mathf.Sqrt(Mathf.Pow(mGDS.StreetSpacing, 2) * 2);

        //                Vector2 vec1 = (currentEdge.Successor.StartCorner.position - currentEdge.StartCorner.position).normalized;
        //                Vector2 vec2 = (currentEdge.Predecessor.StartCorner.position - currentEdge.StartCorner.position).normalized;


        //                if (Vector2.SignedAngle(vec1, vec2) < 0)
        //                {
        //                    vectorLength *= -1;
        //                }

        //                Vector2 bisectingOffset = (vec1 + vec2).normalized * vectorLength;

        //                Vector2 corner = currentEdge.StartCorner.position + bisectingOffset;

        //                corners.Add(new Vector3(corner.x, 0, corner.y));
        //            }

        //            currentEdge = currentEdge.Successor;

        //        } while (currentEdge != facet.IncidentHalfEdge);


        //        GameObject building = new GameObject("Building");
        //        MeshFilter filter = building.AddComponent<MeshFilter>();
        //        ProBuilderMesh mesh = building.AddComponent<ProBuilderMesh>();

        //        mesh.CreateShapeFromPolygon(corners, -50, flipFaces);

        //        Mesh gizmoMesh = new Mesh();
        //        MeshUtility.CopyTo(filter.sharedMesh, gizmoMesh);

        //        Graphics.DrawMesh(gizmoMesh, building.transform.position, building.transform.rotation, mGDS.houseMaterial, 0);

        //        DestroyImmediate(building);

        //    }

        //}

        /// <summary>
        /// Creates Mesh for Noise Visualization and draws Mesh as Gizmo
        /// </summary>
        private static void DrawGhostTerrain(MapGenDataSet mGDS)
        {
            Mesh mesh = new Mesh();

            //Higher maximum Mesh size
            mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            Vector3[] vertices = new Vector3[(mGDS.MapSizeX + 1) * (mGDS.MapSizeZ + 1)];

            for (int i = 0, z = 0; z < mGDS.MapSizeZ + 1; z++)
            {
                for (int x = 0; x < mGDS.MapSizeX + 1; x++)
                {
                    //Add y from Noise
                    vertices[i] = HeightManipulator.ManipulateYToNoise(new Vector3(x, 0, z));
                    i++;
                }
            }

            int[] triangles = new int[mGDS.MapSizeX * mGDS.MapSizeZ * 6];

            int vert = 0;
            int tris = 0;

            //Set Triangles
            for (int z = 0; z < mGDS.MapSizeZ; z++)
            {
                for (int x = 0; x < mGDS.MapSizeX; x++)
                {
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + mGDS.MapSizeX + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + mGDS.MapSizeX + 1;
                    triangles[tris + 5] = vert + mGDS.MapSizeX + 2;

                    vert++;
                    tris += 6;

                }
                vert++;
            }

            //Delete primitive Data from Mesh
            mesh.Clear();


            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();

            //Draw Mesh Gizmo
            Gizmos.color = Color.grey;
            Gizmos.DrawWireMesh(mesh, new Vector3(0, 0, 0));
        }

        #endregion

        #region Private Methods

        //useless if Bottom of MapOutline is used as Ground
        //private void CreateGround()
        //{
        //    GameObject ground = new GameObject("Ground");
        //    ground.AddComponent<MeshFilter>();
        //    ground.AddComponent<MeshRenderer>().material = mGDS.groundMaterial;


        //    Mesh mesh = new Mesh();
        //    mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        //    ground.GetComponent<MeshFilter>().mesh = mesh;




        //    Vector3[] vertices = new Vector3[4];
        //    int[] triangels = new int[6];

        //    vertices[0] = new Vector3(-mGDS.BorderwidthXZ, 0.01f, -mGDS.BorderwidthXZ);
        //    vertices[1] = new Vector3(mGDS.MapSizeX + mGDS.BorderwidthXZ, 0.01f, -mGDS.BorderwidthXZ);
        //    vertices[2] = new Vector3(-mGDS.BorderwidthXZ, 0.01f, mGDS.MapSizeZ + mGDS.BorderwidthXZ);
        //    vertices[3] = new Vector3(mGDS.MapSizeX + mGDS.BorderwidthXZ, 0.01f, mGDS.MapSizeZ + mGDS.BorderwidthXZ);

        //    triangels[0] = 0;
        //    triangels[1] = 2;
        //    triangels[2] = 1;
        //    triangels[3] = 1;
        //    triangels[4] = 2;
        //    triangels[5] = 3;

        //    mesh.Clear();

        //    mesh.vertices = vertices;
        //    mesh.triangles = triangels;

        //    mesh.RecalculateNormals();
        //}

        /// <summary>
        /// Method for Map Generation
        /// </summary>
        private static GameObject CreateMap(MapGenDataSet mGDS)
        {
            RelativeNeighbourhoodGraph graph = new RelativeNeighbourhoodGraph(CreateGraphCorners(mGDS));
            Map map = new Map(graph, mGDS);

            return map._map;
        }

        /// <summary>
        /// Creates Intersection positions in 2d
        /// </summary>
        /// <returns>Vector2[] of 2d Positions</returns>
        private static Vector2[] CreateGraphCorners(MapGenDataSet mGDS)
        {
            Random.InitState(mGDS.Seed);

            int numberOfIntersections = Random.Range(mGDS.IntersectionMin, mGDS.IntersectionMax);
            Vector2[] corners = new Vector2[numberOfIntersections];

            for (int i = 0; i < numberOfIntersections; i++)
            {
                var randomPos = new Vector2();
                bool isOkay;
                do
                {
                    isOkay = true;
                    randomPos = new Vector2(Random.Range(0, mGDS.MapSizeX), Random.Range(0, mGDS.MapSizeZ));

                    for (int j = 0; j < i; j++)
                    {
                        if (Vector2.Distance(corners[j], randomPos) < mGDS.IntersectionSeparation)
                        {
                            isOkay = false;
                        }
                    }

                } while (!isOkay);

                corners[i] = randomPos;
            }

            return corners;

        }

        #endregion

    }
}
