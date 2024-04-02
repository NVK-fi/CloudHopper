using UnityEngine;

namespace Settings
{
	[CreateAssetMenu(fileName = "ProgressionSettings", menuName = "ScriptableObjects/ProgressionSettings", order = 0)]
	public class ProgressionSettings : ScriptableObject
	{
		[SerializeField] private float playerForwardVelocity = .2f;
		[SerializeField] private float playerVerticalVelocity = .05f;
		
		/// <summary>
		/// A progression factor for player's forwards velocity.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public float ForwardVelocity(int hopCount)
		{
			// Cap the progression to a huge number so things won't reach infinity.
			// - I don't expect anyone to get this far.
			hopCount = Mathf.Min(hopCount, 1000);
			
			return 1f + hopCount * playerForwardVelocity;
		}

		/// <summary>
		/// A progression factor for player's vertical velocities.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public float VerticalVelocity(int hopCount)
		{
			// Cap the progression.
			hopCount = Mathf.Min(hopCount, 1000);
			
			return 1f + hopCount * playerVerticalVelocity;
		}
	}
}
