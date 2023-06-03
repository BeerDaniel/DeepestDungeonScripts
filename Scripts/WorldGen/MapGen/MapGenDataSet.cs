using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen
{
    [CreateAssetMenu(fileName = "MapGenDataSet", menuName = "ScriptableObjects/WorldGen/MapGenDataSet", order = 1)]
    public class MapGenDataSet : ScriptableObject
    {
        /// <summary>
        /// X Size of the Map
        /// </summary>
        [Tooltip("X Size of the Map")]
        public int MapSizeX;

        /// <summary>
        /// Z Size of the Map
        /// </summary>
        [Tooltip("Z Size of the Map")]
        public int MapSizeZ;

        /// <summary>
        /// Y Size of the Map
        /// </summary>
        [Tooltip("Y Size of the Map")]
        public int MapSizeY;

        /// <summary>
        /// Empty Space around Map in XZ level
        /// </summary>
        [Tooltip("Empty Space around Map in XZ level")]
        public int BorderwidthXZ;

        /// <summary>
        /// Empty Space between Map and Ground
        /// </summary>
        [Tooltip("Empty Space between Map and Ground")]
        public int OffsetY_Ground;

        /// <summary>
        /// Empty Space between Map and Ceiling
        /// </summary>
        [Tooltip("Empty Space between Map and Ceiling")]
        public int OffsetY_Ceiling;

        /// <summary>
        /// Minimum Number of Intersections
        /// </summary>
        [Tooltip("Minimum Number of Intersections")]
        public int IntersectionMin;

        /// <summary>
        /// Maximum Number of Intersections
        /// </summary>
        [Tooltip("Maximum Number of Intersections")]
        public int IntersectionMax;

        /// <summary>
        /// Minimum space between Intersections
        /// </summary>
        [Tooltip("Minimum space between Intersections")]
        public int IntersectionSeparation;

        /// <summary>
        /// Space between Bridges and some Buildings
        /// </summary>
        [Tooltip("Space between Bridges and some Buildings")]
        [Range(0, float.MaxValue)]
        public float BridgeSpacing;

        /// <summary>
        /// Width of Bridges
        /// </summary>
        [Tooltip("Width of Bridges")]
        [Range(0.1f, float.MaxValue)]
        public float BridgeWidth;

        /// <summary>
        /// Height of an Ingame Room
        /// </summary>
        [Tooltip("Height of an Ingame Room")]
        [Range(1, float.MaxValue)]
        public float RoomHeight;

        /// <summary>
        /// Thickness of Terrain
        /// </summary>
        [Tooltip("Thickness of Terrain")]
        [Range(0.01f, float.MaxValue)]
        public float TerrainThicknes;

        /// <summary>
        /// Levelseed
        /// </summary>
        [Tooltip("Levelseed")]
        public int Seed;

        /// <summary>
        /// Scale of TerrainNoise
        /// </summary>
        [Tooltip("Scale of TerrainNoise")]
        [Range(1, 50000)]
        public int NoiseScale;

        /// <summary>
        /// Level of surface dividing
        /// </summary>
        [Tooltip("Level of surface dividing")]
        public int Smoothness;


    }
}

