namespace Settings
{
	/// <summary>
	/// Contains const values used to prevent typos.
	/// These values have no need to be tweaked.
	/// </summary>
	public static class Constants
	{
		// These keys are used with PlayerPrefs.
		public const string SENSITIVITY_KEY = "Sensitivity";
		public const string HIGH_SCORE_KEY = "HighScore";
		public const string LAST_SCORE_KEY = "LastScore";

		// These are the "The really big number"s preventing floating point errors when progression approaches infinity.
		// - I don't expect anyone to actually reach these.
		public const int MAX_PROGRESSION = 1023;
		public const int MAX_PLATFORM_DISTANCE = 4095;
		public const int MAX_PLAYER_DISTANCE = 65535;
	}
}
