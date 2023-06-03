using Photon.Pun;
using UnityEngine;
using WatStudios.DeepestDungeon.Messaging;
using WatStudios.DeepestDungeon.Networking;

namespace WatStudios.DeepestDungeon.WorldGen
{
    public class WorldGen : MonoBehaviour
    {
        /// <summary>
        /// Scriptable object for MapGen data
        /// </summary>
        [Tooltip("Scriptable object for world gen data")]
        public MapGenDataSet MGDS;

        /// <summary>
        /// Scriptable object for NavGen data
        /// </summary>
        [Tooltip("Scriptable object for nav gen data")]
        public NavGenDataSet NGDS;

        /// <summary>
        /// Scriptable object for EnemyGen data
        /// </summary>
        [Tooltip("Scriptable object for enemy gen data")]
        public EnemyGenDataSet EGDS;

        /// <summary>
        /// Bool for Preview of bridges and intersections
        /// </summary>
        [Tooltip("Bool for Preview of bridges and intersections")]
        public bool MapPreview;

        /// <summary>
        /// Bool for Preview of Heightmap
        /// </summary>
        [Tooltip("Bool for Preview of Heightmap")]
        public bool CreateGhostTerrain;

        // Start is called before the first frame update
        void Start()
        {
            GameObject map = Mapgen.Create(MGDS);
            NavGen.Create(map, NGDS);


            if (PhotonNetwork.IsMasterClient)
            {
                EnemyGen.Create(map, EGDS);
                NetworkManager.Instance.RaiseNetworkEvent(NetworkGameEventCode.GamePlaySceneBuilt,
                    new Photon.Realtime.RaiseEventOptions
                    {
                        Receivers = Photon.Realtime.ReceiverGroup.All
                    }, ExitGames.Client.Photon.SendOptions.SendReliable);
            }
        }

        /// <summary>
        /// OnDrawGizmos is called in editor Window (only Editorbuild)
        /// </summary>
        private void OnDrawGizmos()
        {
            Mapgen.DrawGizmos(MGDS, MapPreview, CreateGhostTerrain);
        }
    }
}
