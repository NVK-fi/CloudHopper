namespace Settings
{
	using UnityEngine;

	/// <summary>
	/// Represents the settings for physics in the game.
	/// </summary>
	[CreateAssetMenu(fileName = "PhysicsSettings", menuName = "ScriptableObjects/PhysicsSettings", order = 1)]
	public class PhysicsSettings : ScriptableObject
	{
		[field: SerializeField] public float ForwardVelocity { get; private set; } = 10f;
		[field: SerializeField] public float Gravity { get; private set; } = 20f;
		[field: SerializeField] public float HopVelocity { get; private set; } = 15f;
		[field: SerializeField] public float DiveVelocity { get; private set; } = 15f;
	}
}
