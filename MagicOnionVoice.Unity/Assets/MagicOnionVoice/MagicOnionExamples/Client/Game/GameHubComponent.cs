using MagicOnionExamples.Client.Network;
using MagicOnionExamples.ServerShared.Game;
using MagicOnionExamples.ServerShared.Network;
using System;
using UnityEngine;

namespace MagicOnionExamples.Client.Game
{
    public class GameHubComponent : MonoBehaviour, IGameHubReceiver
    {
        public static GameHubComponent Instance { get { return _instance; } }

        public delegate void OnMoveHandler(PlayerMoveParameter param);
        public OnMoveHandler OnMovePlayerCharacter;

        public delegate void OnJoinOrLeaveHandler(Player player);
        public OnJoinOrLeaveHandler OnJoin;
        public OnJoinOrLeaveHandler OnLeave;

        public Action AfterJoinGameHub;
        public Action BeforeLeaveGameHub;
        public Action AfterLeaveGameHub;

        private static GameHubComponent _instance;
        private GameHubClient _gameHubClient;

        void Awake()
        {
            _instance = this;

            _gameHubClient = new GameHubClient(this);
            _gameHubClient.AfterJoinHub += () => AfterJoinGameHub?.Invoke();
            _gameHubClient.BeforeLeaveHub += () => BeforeLeaveGameHub?.Invoke();
            _gameHubClient.AfterLeaveHub += () => AfterLeaveGameHub?.Invoke();

            MagicOnionNetwork.RegisterHubClient(_gameHubClient);
        }

        public void MoveAsync(PlayerMoveParameter param)
        {
            if (MagicOnionNetwork.IsConnected)
            {
                _gameHubClient.MoveAsync(param);
            }
        }

        void IGameHubReceiver.OnMove(PlayerMoveParameter param)
        {
            OnMovePlayerCharacter?.Invoke(param);
        }

        void IGameHubReceiver.OnJoin(Player player)
        {
            OnJoin?.Invoke(player);
        }

        void IGameHubReceiver.OnLeave(Player player)
        {
            OnLeave?.Invoke(player);
        }
    }
}
