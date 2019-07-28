using Grpc.Core;
using MagicOnion.Client;
using MagicOnionExamples.Client.Network;
using MagicOnionExamples.ServerShared.Network;
using MagicOnionExamples.ServerShared.Game;
using System;
using System.Threading.Tasks;

namespace MagicOnionExamples.Client.Game
{
    public class GameHubClient : IHubClient
    {
        IGameHub _streamingHub;
        IGameHubReceiver _receiver;

        public Action AfterJoinHub;
        public Action BeforeLeaveHub;
        public Action AfterLeaveHub;

        public GameHubClient(IGameHubReceiver receiver)
        {
            this._receiver = receiver;
        }

        public async void MoveAsync(PlayerMoveParameter param)
        {
            await _streamingHub.MoveAsync(param);
        }

        void IHubClient.ConnectHub(Channel channel)
        {
            _streamingHub = StreamingHubClient.Connect<IGameHub, IGameHubReceiver>(channel, _receiver);
        }

        async Task IHubClient.DisconnectHubAsync()
        {
            await _streamingHub.DisposeAsync();
        }

        Task<JoinResult> IHubClient.JoinHubAsync(string roomName, string playerName, string userId)
        {
            return _streamingHub.JoinAsync(roomName, playerName, userId);
        }

        async void IHubClient.LeaveHubAsync()
        {
            await _streamingHub.LeaveAsync();
        }

        void IHubClient.AfterJoinHub()
        {
            this.AfterJoinHub?.Invoke();
        }

        void IHubClient.BeforeLeaveHub()
        {
            this.BeforeLeaveHub?.Invoke();
        }

        void IHubClient.AfterLeaveHub()
        {
            this.AfterLeaveHub?.Invoke();
        }
    }
}
