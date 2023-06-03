using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace WatStudios.DeepestDungeon.WorldGen
{
    public static class NavGen
    {
        public static void Create(GameObject gameObject, NavGenDataSet nGDS)
        {

            foreach (NavMeshAgent agent in nGDS.NavAgents)
            {
                NavMeshSurface nav = gameObject.AddComponent<NavMeshSurface>();

                nav.agentTypeID = agent.agentTypeID;
                nav.collectObjects = CollectObjects.All;
                
                nav.layerMask = LayerMask.GetMask(new string[] {"Floor"});

                nav.useGeometry = NavMeshCollectGeometry.RenderMeshes;
                nav.defaultArea = 0;
                nav.overrideVoxelSize = false;
                nav.overrideTileSize = false;

                nav.BuildNavMesh();
            }
        }
    }
}
