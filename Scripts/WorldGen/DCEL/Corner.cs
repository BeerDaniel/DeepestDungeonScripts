using System.Collections.Generic;
using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen.DCEL
{
    /// <summary>
    /// Class for Corner representation in a DCEL
    /// </summary>
    public class Corner
    {
        public HalfEdge IncidentHalfEdge { get; set; }
        public Vector2 position { get; set; }

        /// <summary>
        /// Constructor for a DCEL Corner
        /// </summary>
        /// <param name="position"></param>
        public Corner(Vector2 position)
        {
            this.position = position;
        }

    }
}

