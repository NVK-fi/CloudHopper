namespace Settings
{
	using Managers;

	public static class ProgressionMultipliers
	{
		private static readonly ProgressionSettings ProgressionSettings = GameManager.Instance.ProgressionSettings;
		
		/// <summary>
		/// A progression factor for player's forwards velocity.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public static float ForwardsVelocity(int hopCount) => 1f + hopCount * ProgressionSettings.PlayerForwardVelocity;

		/// <summary>
		/// A progression factor for player's vertical velocities.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public static float VerticalVelocity(int hopCount) => 1f + hopCount * ProgressionSettings.PlayerVerticalVelocity;

		/// <summary>
		/// A progression factor for platforms offsets.
		/// Starts from 1 and goes up a small amount after each hop.
		/// </summary>
		public static float PlatformOffset(int platformCount) => 1f + platformCount * ProgressionSettings.PlatformRandomOffset;
	}
}
