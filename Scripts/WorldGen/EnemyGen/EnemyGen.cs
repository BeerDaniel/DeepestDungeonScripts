using UnityEngine;
using UnityEngine.AI;
using WatStudios.DeepestDungeon.Core.EnemyLogic;
using WatStudios.DeepestDungeon.HelperClasses;

namespace WatStudios.DeepestDungeon.WorldGen
{
    public static class EnemyGen
    {

        public static void Create(GameObject gameObject, EnemyGenDataSet eGDS)
        {
            gameObject.AddComponent<RandomNavMeshPoint>();
            int enemyCounter = 0;


            while (enemyCounter < eGDS.enemies)
            {
                EnemyPackDataSet pack = eGDS.Packs[Random.Range(0, eGDS.Packs.Count)];

                Vector3 packSpawnPosition = RandomNavMeshPoint.GetRandomPointOnNavMesh();
                //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = packSpawnPosition;
                SpawnPack(packSpawnPosition, pack, ref enemyCounter);
            }


        }

        private static void SpawnPack(Vector3 packSpawnPosition, EnemyPackDataSet pack, ref int enemyCounter)
        {
            foreach (Enemies enemies in pack.EnemyPack)
            {
                for (int i = 0; i < enemies.amount; i++)
                {
                    Vector3 unitSphereSpawn = Random.insideUnitSphere * pack.PackScattering;
                    unitSphereSpawn += packSpawnPosition;
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(unitSphereSpawn, out hit, pack.PackScattering, NavMesh.AllAreas)) 
                    {
                        EnemyManager.Instance.SpawnEnemy(hit.position, enemies.EnemyPrefab);
                        enemyCounter++;
                    }
                    else
                    {
                        Debug.LogError("NavMeshHit failed.");
                    }                
                }
            }
        }
    }
}
