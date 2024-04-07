using UnityEngine;

namespace Game
{
	using System;
	using Platforms;
	using Settings;

	/// <summary>
	/// Manages the game score.
	/// </summary>
	[RequireComponent(typeof(Game))]
	public class GameScore : MonoBehaviour
	{
		/// <summary>
		/// Get the current in-game score.
		/// </summary>
		public int Current { get; private set; }

		/// <summary>
		/// The event triggered when the score is increased.
		/// </summary>
		/// <remarks>
		/// The integer argument represents the amount of score increased.
		/// </remarks>
		public event Action<int> ScoreIncreased;

		private void OnEnable()
		{
			Game.Instance.Platforms.PlatformsSkipped += OnPlatformsSkipped;
			Game.Instance.Player.TouchedPlatform += OnPlayerTouchedPlatform;
			Game.Instance.Player.Death += OnPlayerDeath;
		}

		private void OnDisable()
		{
			Game.Instance.Platforms.PlatformsSkipped -= OnPlatformsSkipped;
			Game.Instance.Player.TouchedPlatform -= OnPlayerTouchedPlatform;
			Game.Instance.Player.Death -= OnPlayerDeath;
		}

		private void OnPlayerDeath()
		{
			var currentHighScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
			if (Current > currentHighScore) 
				PlayerPrefs.SetInt(Constants.HIGH_SCORE_KEY, Current);
			
			PlayerPrefs.SetInt(Constants.LAST_SCORE_KEY, Current);
			PlayerPrefs.Save();
		}

		// Score 1 point when the player hops.
		private void OnPlayerTouchedPlatform(Platform _)
		{
			Current++;
			ScoreIncreased?.Invoke(1);
		}

		// Score 10 points each time the player skips a platform.
		private void OnPlatformsSkipped(int count)
		{
			var scoreIncrease = 10 * count - 1; 
			Current += scoreIncrease;
			ScoreIncreased?.Invoke(scoreIncrease);
		}
	}
}
