namespace Settings
{
	using UnityEngine;

	[CreateAssetMenu(fileName = "Physics", menuName = "ScriptableObjects/Physics", order = 1)]
	public class PhysicsSettings : ScriptableObject
	{
		[field: SerializeField] public float ForwardVelocity { get; private set; } = 7f;
		[field: SerializeField] public float Gravity { get; private set; } = 16f;
		[field: SerializeField] public float HopVelocity { get; private set; } = 12f;
		[field: SerializeField] public float DiveVelocity { get; private set; } = 12f;
	}
}
