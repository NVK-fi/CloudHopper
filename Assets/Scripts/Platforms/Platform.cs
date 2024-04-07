using UnityEngine;

namespace Platforms
{
	using Random = Random;

	/// <summary>
	/// Represents a platform in the game.
	/// </summary>
	public class Platform : MonoBehaviour
	{
		private void Start()
		{
			SetRandomRotation();
			SetRandomScale();	
		}

		/// <summary>
		/// Teleports the platform to the specified world position.
		/// Also applies a random local rotation and scale.
		/// </summary>
		public void TeleportTo(Vector3 worldPosition)
		{
			transform.position = worldPosition;
			
			SetRandomRotation();
			SetRandomScale();
		}

		private void SetRandomRotation()
		{
			var rotation = Random.Range(0f, 360f);
			transform.localRotation = Quaternion.Euler(0f, rotation, 0f);
		}

		private void SetRandomScale()
		{
			var scale = Random.Range(.75f, 1.25f);
			transform.localScale = Vector3.one * scale;
		}
	}
}