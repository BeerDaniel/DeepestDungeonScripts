using System.Collections.Generic;
using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen
{
    [CreateAssetMenu(fileName = "EnemyPackDataSet", menuName = "ScriptableObjects/WorldGen/EnemyPackDataSet", order = 1)]
    public class EnemyPackDataSet : ScriptableObject
    {
        public float PackScattering;
        public List<Enemies> EnemyPack = new List<Enemies>();
    }
    [System.Serializable]
    public struct Enemies
    {
        public GameObject EnemyPrefab;
        public int amount;
    }
}
