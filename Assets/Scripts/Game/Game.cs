namespace Game
{
	using Controls;
	using Platforms;
	using Player;
	using Settings;
	using UnityEngine;

	/// <summary>
	/// Represents the main game class.
	/// It is a singleton holding references to other classes.
	/// </summary>
	[DefaultExecutionOrder(-1)]
	public class Game : MonoBehaviour
	{
		public static Game Instance { get; private set; }
		[field: SerializeField] public Player Player { get; private set; }
		[field: SerializeField] public PlatformManager Platforms { get; private set; }
		[field: SerializeField] public GameScore Score { get; private set; }
		[field: SerializeField] public PhysicsSettings PhysicsSettings { get; private set; }
		[field: SerializeField] public ProgressionSettings ProgressionSettings { get; private set; }
		public InputAsset Controls { get; private set; }

		private void Awake()
		{
			// Use a singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			if (Player == null)
				Debug.LogError("No Player attached on " + this + "!");
			if (Platforms == null)
				Debug.LogError("No PlatformManager attached on " + this + "!");
			if (Score == null)
				Debug.LogError("No GameScore attached on " + this + "!");
			if (PhysicsSettings == null)
				Debug.LogError("No PhysicsSettings set on " + this + "!");
			if (ProgressionSettings == null)
				Debug.LogError("No ProgressionSettings set on " + this + "!");
			
			Controls = new InputAsset();
		}
		
		private void OnEnable() => Controls.Enable();
		private void OnDisable() => Controls.Disable();
	}
}
