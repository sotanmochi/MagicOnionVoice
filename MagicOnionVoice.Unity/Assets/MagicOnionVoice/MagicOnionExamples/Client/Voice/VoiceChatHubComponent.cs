using MagicOnionExamples.Client.Network;
using MagicOnionExamples.ServerShared.Network;
using MagicOnionExamples.ServerShared.Voice;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace MagicOnionExamples.Client.Voice
{
    public class VoiceChatHubComponent : MonoBehaviour, IVoiceChatHubReceiver
    {
        public static VoiceChatHubComponent Instance { get { return _instance; } }

        [SerializeField]
        private GameObject SpeakerPrefab;
        [SerializeField]
        private GameObject RemoteVoiceRoot;

        public delegate void OnInstantiateSpeakerHandler(int actorNumber, GameObject go);
        public OnInstantiateSpeakerHandler OnInstantiateSpeakerObject;

        private Dictionary<int, Speaker> _remoteVoices;
        private SynchronizationContext _unityMainThread;

        private static VoiceChatHubComponent _instance;
        private VoiceChatHubClient _voiceChatHubClient;

        void Awake()
        {
            _instance = this;
            _remoteVoices = new Dictionary<int, Speaker>();
            _unityMainThread = SynchronizationContext.Current;

            _voiceChatHubClient = new VoiceChatHubClient(this);
            _voiceChatHubClient.AfterLeaveHub += AfterLeaveVoiceChatHub;

            MagicOnionNetwork.RegisterHubClient(_voiceChatHubClient);
        }

        public void SendFrame(byte[] data, int length)
        {
            if (MagicOnionNetwork.IsJoined)
            {
                VoiceMessage message = new VoiceMessage();
                message.SenderActorNumber = MagicOnionNetwork.LocalPlayer.ActorNumber;
                message.SenderPlayerName = MagicOnionNetwork.LocalPlayer.Name;
                message.EncodedData = data;
                message.DataLength = length;
                _voiceChatHubClient.SendMessageExceptSelfAsync(message);
            }
        }

        private void InstantiateSpeakerObject(int actorNumber, string playerName)
        {
            GameObject go = GameObject.Instantiate(SpeakerPrefab, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(RemoteVoiceRoot.transform);
            go.name = "RemoteVoice[" + actorNumber + "]_" + playerName;

            _remoteVoices[actorNumber] = go.GetComponentInChildren<Speaker>();

            OnInstantiateSpeakerObject?.Invoke(actorNumber, go);
        }

        void IVoiceChatHubReceiver.OnReceivedMessage(VoiceMessage msg)
        {
            Speaker remoteVoice;
            _remoteVoices.TryGetValue(msg.SenderActorNumber, out remoteVoice);
            if (remoteVoice != null)
            {
                remoteVoice.ReceiveBytes(msg.EncodedData, msg.DataLength);
            }
            else
            {
                InstantiateSpeakerObject(msg.SenderActorNumber, msg.SenderPlayerName);
            }
        }

        void IVoiceChatHubReceiver.OnJoin(Player player)
        {
            InstantiateSpeakerObject(player.ActorNumber, player.Name);
        }

        void IVoiceChatHubReceiver.OnLeave(Player player)
        {
            Speaker remoteVoice;
            _remoteVoices.TryGetValue(player.ActorNumber, out remoteVoice);
            if (remoteVoice != null)
            {
                DestroyImmediate(remoteVoice.gameObject);
                _remoteVoices.Remove(player.ActorNumber);
            }
        }

        private void AfterLeaveVoiceChatHub()
        {
            Debug.Log("AfterLeaveVoiceChatHub");
            _unityMainThread.Post((_) =>
            {
                foreach(var remoteVoice in _remoteVoices.Values)
                {
                    DestroyImmediate(remoteVoice.gameObject);
                }
                _remoteVoices.Clear();
            }
            , null);
        }
    }
}
