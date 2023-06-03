using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen.DCEL
{
    /// <summary>
    /// Class for Facet representation in a DCEL
    /// </summary>
    public class Facet
    {
        public HalfEdge IncidentHalfEdge { get; set; }
        public bool IsOutline { get; set; }

        /// <summary>
        /// Constructor for a DCEL Facet
        /// </summary>
        public Facet()
        {
            //If not explicitly set every new Facet is a not Outline
            IsOutline = false;
        }

        #region Public Methods

        /// <summary>
        /// Get Surface of the Facet
        /// </summary>
        /// <returns>Surface of Facet</returns>
        public float GetSurface()
        {
            float surface = 0;

            HalfEdge currenHalfEdge = IncidentHalfEdge;

            //Shoelace Formula (dt.: gaußsche Trapetzformel)
            do
            {
                Vector3 pointA = currenHalfEdge.StartCorner.position;
                Vector3 pointB = currenHalfEdge.TwinHalfEdge.StartCorner.position;

                surface += (pointA.y + pointB.y) * (pointA.x - pointB.x);

                currenHalfEdge = currenHalfEdge.Successor;

            } while (currenHalfEdge != IncidentHalfEdge);

            return Mathf.Abs(surface / 2.0f);
        }

        /// <summary>
        /// Get all Corners touching the Facet
        /// </summary>
        /// <returns>List of all Corners incident to the Facet</returns>
        public List<Corner> GetAllIncidentCorners()
        {
            List<Corner> corners = new List<Corner>();

            HalfEdge currentEdge = IncidentHalfEdge;

            do
            {
                corners.Add(currentEdge.StartCorner);
                currentEdge = currentEdge.Successor;

            } while (currentEdge != IncidentHalfEdge);

            return corners;
        }

        /// <summary>
        /// Get all HalfEdges touching the Facet
        /// </summary>
        /// <returns>List of all HalfEdges incident to the Facet</returns>
        public List<HalfEdge> GetAllIncidentHalfEdges()
        {
            List<HalfEdge> edges = new List<HalfEdge>();

            HalfEdge currentEdge = IncidentHalfEdge;

            do
            {
                edges.Add(currentEdge);
                currentEdge = currentEdge.Successor;

            } while (currentEdge != IncidentHalfEdge);

            return edges;
        }

        #endregion
    }
}
