using UnityEngine;

namespace Settings
{
	using Managers;

	[CreateAssetMenu(fileName = "ProgressionSettings", menuName = "ScriptableObjects/ProgressionSettings", order = 0)]
	public class ProgressionSettings : ScriptableObject
	{
		[SerializeField] private float playerForwardVelocity = .2f;
		[SerializeField] private float playerVerticalVelocity = .05f;
		[SerializeField] private float platformRandomOffset = .5f;
		
		/// <summary>
		/// A progression factor for player's forwards velocity.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public float ForwardVelocity(int hopCount) => 1f + hopCount * playerForwardVelocity;

		/// <summary>
		/// A progression factor for player's vertical velocities.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public float VerticalVelocity(int hopCount) => 1f + hopCount * playerVerticalVelocity;

		/// <summary>
		/// A progression factor for platforms offsets.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public float PlatformOffset(int platformCount) => 1f + platformCount * platformRandomOffset;
	}
}
