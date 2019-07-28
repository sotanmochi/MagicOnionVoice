using MessagePack;

namespace MagicOnionExamples.ServerShared.Network
{
    [MessagePackObject]
    public class JoinResult
    {
        [Key(0)]
        public Player LocalPlayer { get; set; }
    }
}
