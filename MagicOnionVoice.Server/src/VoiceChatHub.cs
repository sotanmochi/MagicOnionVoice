using System;
using System.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Server.Hubs;
using MagicOnionExamples.ServerShared.Network;
using MagicOnionExamples.ServerShared.Voice;

namespace MagicOnionExamples.Server
{
    class VoiceChatHub : StreamingHubBase<IVoiceChatHub, IVoiceChatHubReceiver>, IVoiceChatHub
    {
        IGroup group;
        Player self;
        string currentRoom;

        public async Task SendMessageExceptSelfAsync(VoiceMessage message)
        {
            BroadcastExceptSelf(group).OnReceivedMessage(message);
            await Task.CompletedTask;
        }

        public async Task<JoinResult> JoinAsync(string roomName, string playerName, string userId)
        {
            Guid connectionId = this.Context.ContextId;
            GrpcEnvironment.Logger.Debug("VoiceChatHub - ConnectionId: " + connectionId);

            self = RoomManager.Instance.JoinOrCreateRoom(roomName, playerName, userId);
            Player[] roomPlayers = RoomManager.Instance.GetRoom(roomName).GetPlayers();

            GrpcEnvironment.Logger.Debug("VoiceChatHub - PlayerCount: " + roomPlayers.Length);
            currentRoom = roomName;

            if (self.ActorNumber >= 0)
            {
                this.group = await Group.AddAsync(roomName);
                BroadcastExceptSelf(group).OnJoin(self);
                // Broadcast(group).OnJoin(self);
            }

            return new JoinResult() { LocalPlayer = self };
        }

        public async Task LeaveAsync()
        {
            GrpcEnvironment.Logger.Debug("LeavAsync @VoiceChatHub");

            RoomManager.Instance.LeaveRoom(self.UserId);
            Player[] roomPlayers = RoomManager.Instance.GetRoom(currentRoom).GetPlayers();
            GrpcEnvironment.Logger.Debug("VoiceChatHub - PlayerCount: " + roomPlayers.Length);

            await group.RemoveAsync(this.Context);
            Broadcast(group).OnLeave(self);
        }
    }
}
