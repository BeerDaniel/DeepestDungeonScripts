using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WatStudios.DeepestDungeon.WorldGen
{
    [CreateAssetMenu(fileName = "NavGenDataSet", menuName = "ScriptableObjects/WorldGen/NavGenDataSet", order = 1)]
    public class NavGenDataSet : ScriptableObject
    {
        public List<NavMeshAgent> NavAgents;
    }
}