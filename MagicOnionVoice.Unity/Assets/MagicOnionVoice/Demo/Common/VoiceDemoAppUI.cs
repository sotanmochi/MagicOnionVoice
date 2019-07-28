using MagicOnionExamples.Client.Network;
using UnityEngine;
using UnityEngine.UI;

namespace MagicOnionExamples.Demo.Voice
{
    public class VoiceDemoAppUI : MonoBehaviour
    {
        [SerializeField] InputField Host;
        [SerializeField] InputField Port;
        [SerializeField] Button ConnectButton;
        [SerializeField] Text ConnectionResult;

        [SerializeField] InputField RoomName;
        [SerializeField] InputField PlayerName;
        [SerializeField] Button JoinButton;
        [SerializeField] Button LeaveButton;
        [SerializeField] Text JoinResult;

        void Start()
        {
            ConnectButton.onClick.AddListener(OnConnectClicked);
            JoinButton.onClick.AddListener(OnJoinClicked);
            LeaveButton.onClick.AddListener(OnLeaveClicked);
        }

        void OnConnectClicked()
        {
            MagicOnionNetwork.Connect(Host.text, int.Parse(Port.text));
            ConnectionResult.text = "State: " + MagicOnionNetwork.ConnectionState;

            Debug.Log("*** OnConnectClicked @VoiceDemoAppUI ***");
            Debug.Log("Connected: " + MagicOnionNetwork.IsConnected);
            Debug.Log("State: " + MagicOnionNetwork.ConnectionState);
        }

        async void OnJoinClicked()
        {
            string userId = System.Guid.NewGuid().ToString();
            bool result = await MagicOnionNetwork.JoinAsync(RoomName.text, PlayerName.text, userId);
            JoinResult.text = "IsJoined: " + MagicOnionNetwork.IsJoined;

            Debug.Log("*** OnJoinClicked @VoiceDemoAppUI ***");
            Debug.Log("Join success: " + result);
        }

        async void OnLeaveClicked()
        {
            await MagicOnionNetwork.LeaveAsync();
            JoinResult.text = "IsJoined: " + MagicOnionNetwork.IsJoined;
        }
    }
}