using MagicOnionExamples.Client.Game;
using MagicOnionExamples.Client.Voice;
using MagicOnionExamples.ServerShared.Game;
using MagicOnionExamples.ServerShared.Network;
using System.Collections.Generic;
using UnityEngine;

namespace MagicOnionExamples.Demo
{
    public class RemotePlayerManager : MonoBehaviour
    {
        public static RemotePlayerManager Instance { get { return _instance; } }

        private Dictionary<int, GameObject> _remotePlayers;
        private static RemotePlayerManager _instance;

        void Awake()
        {
            _instance = this;
            _remotePlayers = new Dictionary<int, GameObject>();
        }

        void Start()
        {
            VoiceChatHubComponent.Instance.OnInstantiateSpeakerObject += RegisterRemotePlayerObject;
            GameHubComponent.Instance.OnMovePlayerCharacter += OnMoveRemotePlayerCharacter;
            GameHubComponent.Instance.OnLeave += OnLeave;
        }

        public void RegisterRemotePlayerObject(int actorNumber, GameObject playerObject)
        {
            _remotePlayers.Add(actorNumber, playerObject);
        }

        private void OnMoveRemotePlayerCharacter(PlayerMoveParameter param)
        {
            GameObject remotePlayer;
            _remotePlayers.TryGetValue(param.ActorNumber, out remotePlayer);
            if (remotePlayer != null)
            {
                remotePlayer.transform.position = param.Position;
                remotePlayer.transform.rotation = param.Rotation;
            }
        }

        private void OnLeave(Player player)
        {
            GameObject remotePlayer;
            _remotePlayers.TryGetValue(player.ActorNumber, out remotePlayer);
            if (remotePlayer != null)
            {
                _remotePlayers.Remove(player.ActorNumber);
            }
        }
    }
}
