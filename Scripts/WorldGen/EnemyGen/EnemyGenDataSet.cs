using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WatStudios.DeepestDungeon.WorldGen
{
    [CreateAssetMenu(fileName = "EnemyGenDataSet", menuName = "ScriptableObjects/WorldGen/EnemyGenDataSet", order = 1)]
    public class EnemyGenDataSet : ScriptableObject
    {
        public List<EnemyPackDataSet> Packs = new List<EnemyPackDataSet>();
        public int enemies;
    }
}
