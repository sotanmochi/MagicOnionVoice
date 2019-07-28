using Grpc.Core;
using MagicOnionExamples.ServerShared.Network;
using System.Threading.Tasks;

namespace MagicOnionExamples.Client.Network
{
    public interface IHubClient
    {
        void ConnectHub(Channel channel);
        Task DisconnectHubAsync();
        Task<JoinResult> JoinHubAsync(string roomName, string playerName, string userId);
        void LeaveHubAsync();
        void AfterJoinHub();
        void BeforeLeaveHub();
        void AfterLeaveHub();
    }
}
