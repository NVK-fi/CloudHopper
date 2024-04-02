using UnityEngine;

namespace Platforms
{
	using Random = Random;

	public class Platform : MonoBehaviour
	{
		private void Start()
		{
			SetRandomPlatformRotation();
			SetRandomPlatformScale();
		}

		/// <summary>
		/// Moves the platform to the specified world position.
		/// Also applies a random local rotation and scale.
		/// </summary>
		public void TeleportTo(Vector3 worldPosition)
		{
			transform.position = worldPosition;
			
			SetRandomPlatformRotation();
			SetRandomPlatformScale();
		}

		/// <summary>
		/// Rotates the platform randomly around its Y-axis.
		/// </summary>
		private void SetRandomPlatformRotation()
		{
			var rotation = Random.Range(0f, 360f);
			transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
		}

		/// <summary>
		/// Sets a random scale.
		/// </summary>
		private void SetRandomPlatformScale()
		{
			var scale = Random.Range(.75f, 1.25f);
			transform.localScale = Vector3.one * scale;
		}
	}
}