using MessagePack;
using UnityEngine;

namespace MagicOnionExamples.ServerShared.Game
{
    [MessagePackObject]
    public class PlayerMoveParameter
    {
        [Key(0)]
        public int ActorNumber { get; set; }
        [Key(1)]
        public string PlayerName { get; set; }
        [Key(2)]
        public Vector3 Position { get; set; }
        [Key(3)]
        public Quaternion Rotation { get; set; }
    }
}
