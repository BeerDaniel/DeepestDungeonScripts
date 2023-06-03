using System.Collections.Generic;
using UnityEngine;
using WatStudios.DeepestDungeon.WorldGen.DCEL;

namespace WatStudios.DeepestDungeon.WorldGen
{
    /// <summary>
    /// Class for RelativeNeighbourhoodGraph in form of DCEL
    /// </summary>
    public class RelativeNeighbourhoodGraph
    {
        #region Public Readonly Fields
        public readonly List<HalfEdge> halfEdges;
        public readonly List<Corner> corners;
        public readonly List<Facet> facets;
        #endregion

        /// <summary>
        /// Constructor for DCEL RNG
        /// </summary>
        /// <param name="cornerPoints">Positions of Graph corners</param>
        public RelativeNeighbourhoodGraph(Vector2[] cornerPoints)
        {
            halfEdges = new List<HalfEdge>();
            corners = new List<Corner>();
            facets = new List<Facet>();

            //Add all Corners
            foreach (var cornerPoint in cornerPoints)
            {
                corners.Add(new Corner(cornerPoint));
            }

            //Create Graph
            RNG();

        }

        #region Private Methods

        /// <summary>
        /// Create RNG DCEL
        /// </summary>
        private void RNG()
        {
            //For each Pair of Corners search if there is a third Corner that is closer to both Corners than the pairs Distance 
            for (int i = 0; i < corners.Count; i++)
            {
                for (int j = i + 1; j < corners.Count; j++)
                {
                    float neighbourDistance = Vector2.Distance(corners[i].position, corners[j].position);
                    bool conflict = false;

                    for (int h = 0; h < corners.Count; h++)
                    {
                        if (h != i && h != j)
                        {

                            if (neighbourDistance > Vector2.Distance(corners[i].position, corners[h].position))
                            {
                                if (neighbourDistance > Vector2.Distance(corners[j].position, corners[h].position))
                                {
                                    //if a third corner is closer than the two Other Corners Distance there is a conflict
                                    conflict = true;
                                    break;
                                }
                            }
                        }
                    }

                    //if there is no conflict connect the two corners via Half Edges
                    if (!conflict)
                    {
                        //Create Edge Pair
                        HalfEdge h1 = new HalfEdge(corners[i]);
                        HalfEdge h2 = new HalfEdge(corners[j]);

                        //Set Twins
                        h1.TwinHalfEdge = h2;
                        h2.TwinHalfEdge = h1;

                        //Set as new Incident
                        corners[i].IncidentHalfEdge = h1;
                        corners[j].IncidentHalfEdge = h2;

                        //Add to Edges
                        halfEdges.Add(h1);
                        halfEdges.Add(h2);
                    }
                }
            }

            //SetMissing HalfEdge Properties
            QualifyEdgeInfo();
            GenerateFacets();
            SetOutline();
        }

        /// <summary>
        /// Set Missing Edge Properties
        /// </summary>
        private void QualifyEdgeInfo()
        {
            foreach (var halfEdge in halfEdges)
            {
                SetHalfEdgeSuccessor(halfEdge);
            }
        }

        /// <summary>
        /// Find and Set Successor of half Edge in DCEL
        /// </summary>
        /// <param name="halfEdge"></param>
        private void SetHalfEdgeSuccessor(HalfEdge halfEdge)
        {
            if (halfEdges.Contains(halfEdge) && halfEdges.Contains(halfEdge.TwinHalfEdge))
            {
                List<HalfEdge> successors = halfEdges.FindAll(e => e.StartCorner.Equals(halfEdge.TwinHalfEdge.StartCorner));

                if (successors.Count > 0)
                {
                    //only for compiler! temp is going to be replaced 100%
                    HalfEdge successor = halfEdge;
                    
                    if (successors.Count == 1)
                    {
                        successor = halfEdge.TwinHalfEdge; 
                    }
                    else
                    {
                        float minAngle = float.MaxValue;

                        successors.Remove(halfEdge.TwinHalfEdge);

                        //find next HalfEdge via Angle
                        foreach (var edge in successors)
                        {
                            var vec1 = halfEdge.StartCorner.position - halfEdge.TwinHalfEdge.StartCorner.position;
                            var vec2 = edge.TwinHalfEdge.StartCorner.position - edge.StartCorner.position;

                            Vector3 v1 = new Vector3(vec1.x, 0, vec1.y);
                            Vector3 v2 = new Vector3(vec2.x, 0, vec2.y);


                            float angle = (360 + Vector3.SignedAngle(v1, v2, Vector3.up)) % 360;

                            if (angle < minAngle)
                            {
                                successor = edge;
                                minAngle = angle;
                            }
                        }
                    }

                    //if (successor.Predecessor != null)
                    //{
                    //    successor.Predecessor.Successor = halfEdge.TwinHalfEdge;
                    //    halfEdge.TwinHalfEdge.Predecessor = successor.Predecessor;
                    //}


                    halfEdge.Successor = halfEdges.Find(e => e == successor);
                    halfEdge.Successor.Predecessor = halfEdge;


                }
                else
                {
                    Debug.Log("No Possible succesors.");
                }
            }
            else
            {
                Debug.Log("HalfEdge / TwinHalfEdge not part of List<halfEdge> halfEdges.");
            }
        }

        /// <summary>
        /// Find HalfEdge Cycles and generate Facets
        /// </summary>
        private void GenerateFacets()
        {
            List<HalfEdge> usedEdges = new List<HalfEdge>();

            foreach (var halfEdge in halfEdges)
            {
                if (!usedEdges.Contains(halfEdge))
                {
                    HalfEdge currentEdge = halfEdge;

                    Facet newFacet = new Facet();

                    //Cycle through succesors til at start HalfEdge again
                    do
                    {
                        usedEdges.Add(currentEdge);
                        currentEdge.Facet = newFacet;
                        currentEdge = currentEdge.Successor;

                    } while (currentEdge != halfEdge);

                    newFacet.IncidentHalfEdge = halfEdge;

                    facets.Add(newFacet);
                }

            }
        }

        /// <summary>
        /// Set Facet with Maximum Surfaceare as Outline
        /// </summary>
        private void SetOutline()
        {
            float maxSurfaceSize = 0;

            Facet outline = new Facet();

            foreach (var facet in facets)
            {
                float surface = facet.GetSurface();

                if (surface >= maxSurfaceSize)
                {
                    maxSurfaceSize = surface;
                    outline = facet;
                }
            }

            outline.IsOutline = true;
        }

    }

    #endregion
}