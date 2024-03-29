using UnityEngine;

namespace Settings
{
	using Managers;

	[CreateAssetMenu(fileName = "ProgressionSettings", menuName = "ScriptableObjects/ProgressionSettings", order = 0)]
	public class ProgressionSettings : ScriptableObject
	{
		[field: SerializeField] public float PlayerForwardVelocity { get; private set; } = .2f;
		[field: SerializeField] public float PlayerVerticalVelocity { get; private set; } = .05f;
		[field: SerializeField] public float PlatformRandomOffset { get; private set; } = .5f;
	}
}
