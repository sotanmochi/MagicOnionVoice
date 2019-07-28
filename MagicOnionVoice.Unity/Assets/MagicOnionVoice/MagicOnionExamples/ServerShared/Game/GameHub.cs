using MagicOnion;
using MagicOnionExamples.ServerShared.Network;
using System.Threading.Tasks;

namespace MagicOnionExamples.ServerShared.Game
{
    public interface IGameHub : IStreamingHub<IGameHub, IGameHubReceiver>
    {
        Task MoveAsync(PlayerMoveParameter param);
        Task<JoinResult> JoinAsync(string roomName, string playerName, string userId);
        Task LeaveAsync();
    }

    public interface IGameHubReceiver
    {
        void OnMove(PlayerMoveParameter param);
        void OnJoin(Player player);
        void OnLeave(Player player);
    }
}
