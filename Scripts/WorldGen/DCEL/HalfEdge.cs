using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WatStudios.DeepestDungeon.WorldGen.DCEL
{
    /// <summary>
    /// Class for HalfEdge representation in a DCEL
    /// </summary>
    public class HalfEdge
    {
        public Corner StartCorner { get; set; }
        public HalfEdge TwinHalfEdge { get; set; }
        public Facet Facet { get; set; }
        public HalfEdge Predecessor { get; set; }
        public HalfEdge Successor { get; set; }

        /// <summary>
        /// Constructor for a DCEL HalfEdge
        /// </summary>
        /// <param name="startCorner">Indcident Corner HalfEdge is connected to</param>
        public HalfEdge(Corner startCorner)
        {
            StartCorner = startCorner;
        }
    }
}
