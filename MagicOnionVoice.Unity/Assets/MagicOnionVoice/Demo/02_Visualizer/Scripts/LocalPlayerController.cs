using MagicOnionExamples.Client.Game;
using MagicOnionExamples.Client.Network;
using MagicOnionExamples.ServerShared.Game;
using UnityEngine;

namespace MagicOnionExamples.Demo
{
	public class LocalPlayerController : MonoBehaviour
	{
		public float moveSpeed = 3.5f;
		public float rotationSpeed = 180f;
		
		void Update()
		{
			Vector3 direction = Input.GetAxis("Vertical") * transform.forward + Input.GetAxis("Horizontal") * transform.right;		
			if(direction.sqrMagnitude > 0.01f)
			{
				Vector3 forward = Vector3.Slerp(
					transform.forward,
					direction,
					rotationSpeed * Time.deltaTime / Vector3.Angle(transform.forward, direction)
				);
				transform.LookAt(transform.position + forward);
			}
			transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);

			if (MagicOnionNetwork.IsJoined)
			{
				PlayerMoveParameter param = new PlayerMoveParameter();
				param.ActorNumber = MagicOnionNetwork.LocalPlayer.ActorNumber;
				param.PlayerName = MagicOnionNetwork.LocalPlayer.Name;
				param.Position = transform.position;
				param.Rotation = transform.rotation;

				GameHubComponent.Instance.MoveAsync(param);
			}
		}
	}
}
