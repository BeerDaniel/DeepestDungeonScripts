using System.Collections.Generic;
using UnityEngine;
using WatStudios.DeepestDungeon.WorldGen.DCEL;
using System;
using System.Linq;
using WatStudios.DeepestDungeon.Messaging;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for Creating Game World from RNG DCEL
    /// </summary>
    public class Map
    {
        /// <summary>
        /// GameObject as parent of all Map parts
        /// </summary>
        public readonly GameObject _map;

        #region Private Fields       
        /// <summary>
        /// Corresponding Realtive Neighbourhood Graph of Map
        /// </summary>
        private RelativeNeighbourhoodGraph _rng;
        /// <summary>
        /// WorldGen DataObject 
        /// </summary>
        private MapGenDataSet _mGDS;

        #endregion

        /// <summary>
        /// Constructor for Map
        /// </summary>
        /// <param name="rng">Relative Neighbourhood Graph</param>
        /// <param name="heightManipulator">Height Manipulator</param>
        /// <param name="mGDS">WorldGenDataScriptableObject</param>
        public Map(RelativeNeighbourhoodGraph rng, MapGenDataSet mGDS)
        {
            _rng = rng;
            _mGDS = mGDS;

            _map = new GameObject("Map");

            CreateIntersections(_rng.corners);
            CreateBridges(_rng.halfEdges);
            CreateAreas(_rng.facets);
            SetSpawnPoint(_rng.corners);
        }

        #region Private Methods

        /// <summary>
        /// Create Map Intersections out of DCEL corner
        /// </summary>
        /// <param name="corners">List of corners to create Intersections from</param>
        private void CreateIntersections(List<Corner> corners)
        {
            //Create Parent Object and set as Child of Map
            GameObject intersections = new GameObject("Intersections");
            intersections.transform.parent = _map.transform;

            //Get all Types derived from Intersection except for dead End
            Type[] types = typeof(Intersection).Assembly.GetTypes();
            types = types.Where(e => e.IsSubclassOf(typeof(Intersection)) && e != typeof(DeadEnd)).ToArray();

            foreach (var corner in corners)
            {
                int incidentEdges = _rng.halfEdges.Where(e => e.StartCorner == corner).ToList().Count;

                Type type;
                Intersection intersection;

                //Create Random Intersection if more than 1 Bridge, else create Dead End
                if (incidentEdges > 1)
                {
                    type = types.ElementAt(UnityEngine.Random.Range(0, types.Count()));
                }
                else
                {
                    type = typeof(DeadEnd);
                }

                var areaParams = new object[] { };

                var conInfo = type.GetConstructor(Type.EmptyTypes);
                intersection = conInfo.Invoke(areaParams) as Intersection;


                intersection.Init(corner, _mGDS);
                intersection.gameObject.transform.parent = intersections.transform;
            }
        }

        /// <summary>
        /// Create Map Bridges out of DCEL halfEdges
        /// </summary>
        /// <param name="halfEdges">List of corners to create Bridges from</param>
        private void CreateBridges(List<HalfEdge> halfEdges)
        {
            //Create Parent Object and set as Child of Map
            GameObject bridges = new GameObject("Bridges");
            bridges.transform.parent = _map.transform;

            //Get all Types derived from Bridge
            Type[] types = typeof(Bridge).Assembly.GetTypes();
            types = types.Where(e => e.IsSubclassOf(typeof(Bridge))).ToArray();

            List<HalfEdge> alreadyVisited = new List<HalfEdge>();

            foreach (var halfEdge in halfEdges)
            {
                if (!alreadyVisited.Contains(halfEdge))
                {
                    Bridge bridge;

                    //Create Bridge of Random Bridgetype
                    Type type = types.ElementAt(UnityEngine.Random.Range(0, types.Count()));
                    var bridgeParams = new object[] { };

                    var conInfo = type.GetConstructor(Type.EmptyTypes);
                    bridge = conInfo.Invoke(bridgeParams) as Bridge;


                    bridge.Init(halfEdge, halfEdge.TwinHalfEdge, _mGDS);
                    bridge.gameObject.transform.parent = bridges.transform;

                    alreadyVisited.Add(halfEdge);
                    alreadyVisited.Add(halfEdge.TwinHalfEdge);
                }
            }
        }

        /// <summary>
        /// Create Map Area out of DCEL facets
        /// </summary>
        /// <param name="facets"></param>
        private void CreateAreas(List<Facet> facets)
        {
            //Create Parent Object and set as Child of Map
            GameObject areas = new GameObject("Areas");
            areas.transform.parent = _map.transform;

            //Get all Types derived from Area except for Outline
            Type[] types = typeof(Area).Assembly.GetTypes();
            types = types.Where(e => e.IsSubclassOf(typeof(Area)) && e != typeof(Outline)).ToArray();

            foreach (var facet in facets)
            {
                Area area;

                //if Outline create Outline Area, else Random Type of Areas
                if (facet.IsOutline)
                {
                    area = new Outline();

                }
                else
                {
                    Type type = types.ElementAt(UnityEngine.Random.Range(0, types.Count()));
                    var areaParams = new object[] { };

                    var conInfo = type.GetConstructor(Type.EmptyTypes);
                    area = conInfo.Invoke(areaParams) as Area;
                }

                area.Init(facet, _mGDS);
                area.gameObject.transform.parent = areas.transform;
            }
        }

       

        /// <summary>
        /// Set SpawnPoint for Players
        /// </summary>
        /// <param name="corners">List of corners as possible Spawn Points</param>
        private void SetSpawnPoint(List<Corner> corners)
        {
            Vector3 spawnPosition = HeightManipulator.TransformToVec3Noise(corners[UnityEngine.Random.Range(0, corners.Count())].position);
            spawnPosition += new Vector3(0, 2, 0);

            MessageHub.SendMessage(MessageType.SpawnPoint, spawnPosition);
        }

        #endregion
    }
}
