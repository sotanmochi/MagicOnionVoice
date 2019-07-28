using MessagePack;

namespace MagicOnionExamples.ServerShared.Voice
{
    [MessagePackObject]
    public class VoiceMessage
    {
        [Key(0)]
        public int SenderActorNumber { get; set; }
        [Key(1)]
        public string SenderPlayerName { get; set; }
        [Key(2)]
        public byte[] EncodedData { get; set; }
        [Key(3)]
        public int DataLength { get; set; }
    }
}
