using Grpc.Core;
using MagicOnion.Client;
using MagicOnionExamples.Client.Network;
using MagicOnionExamples.ServerShared.Network;
using MagicOnionExamples.ServerShared.Voice;
using System;
using System.Threading.Tasks;

namespace MagicOnionExamples.Client.Voice
{
    public class VoiceChatHubClient : IHubClient
    {
        IVoiceChatHub _streamingHub;
        IVoiceChatHubReceiver _receiver;

        public Action AfterJoinHub;
        public Action BeforeLeaveHub;
        public Action AfterLeaveHub;

        public VoiceChatHubClient(IVoiceChatHubReceiver receiver)
        {
            this._receiver = receiver;
        }

        public async void SendMessageExceptSelfAsync(VoiceMessage message)
        {
            await _streamingHub.SendMessageExceptSelfAsync(message);
        }

        void IHubClient.ConnectHub(Channel channel)
        {
            _streamingHub = StreamingHubClient.Connect<IVoiceChatHub, IVoiceChatHubReceiver>(channel, _receiver);
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
