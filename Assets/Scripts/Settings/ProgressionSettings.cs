using UnityEngine;

namespace Settings
{
	/// <summary>
	/// ScriptableObject class that stores progression settings for the player.
	/// Each value is used as an increment.
	/// </summary>
	[CreateAssetMenu(fileName = "ProgressionSettings", menuName = "ScriptableObjects/ProgressionSettings", order = 0)]
	public class ProgressionSettings : ScriptableObject
	{
		[SerializeField] private float forwardIncrement = .015f;
		[SerializeField] private float verticalIncrement = .007f;
		
		/// <summary>
		/// Calculates the forward progression multiplier at given hop count.
		/// - Starts from 1 and goes up an increment after each hop.
		/// </summary>
		public float ForwardMultiplier(int hopCount)
		{
			// Cap the progression to a huge number so things won't reach infinity.
			// - I don't expect anyone to get this far.
			hopCount = Mathf.Min(hopCount, Constants.MAX_PROGRESSION);
			return 1f + hopCount * forwardIncrement;
		}
		
		/// <summary>
		/// Calculates the vertical progression multiplier at given hop count.
		/// - Starts from 1 and goes up an increment after each hop.
		/// </summary>
		public float VerticalMultiplier(int hopCount)
		{
			hopCount = Mathf.Min(hopCount, Constants.MAX_PROGRESSION);
			return 1f + hopCount * verticalIncrement;
		}
	}
}
