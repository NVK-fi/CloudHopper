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
		[field: SerializeField] public float ForwardIncrement { get; private set; } = .015f;
		[field: SerializeField] public float VerticalIncrement { get; private set; } = .007f;
	}
}
