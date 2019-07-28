using MagicOnion;
using MagicOnionExamples.ServerShared.Network;
using System.Threading.Tasks;

namespace MagicOnionExamples.ServerShared.Voice
{
    public interface IVoiceChatHub : IStreamingHub<IVoiceChatHub, IVoiceChatHubReceiver>
    {
        Task SendMessageExceptSelfAsync(VoiceMessage message);
        Task<JoinResult> JoinAsync(string roomName, string playerName, string userId);
        Task LeaveAsync();
    }

    public interface IVoiceChatHubReceiver
    {
        void OnReceivedMessage(VoiceMessage message);
        void OnJoin(Player player);
        void OnLeave(Player player);
    }
}
