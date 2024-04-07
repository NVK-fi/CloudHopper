namespace Game
{
	using System.Collections;
	using Platforms;
	using Player;
	using UI;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using UnityEngine.SceneManagement;

	[DefaultExecutionOrder(-1)]
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }
		[field: SerializeField] public PlatformManager Platforms { get; private set; }
		[field: SerializeField] public GameScore Score { get; private set; }

		[SerializeField] private ImageFader imageFader;

		private void Awake()
		{
			// Singleton pattern.
			if (Instance == null)
				Instance = this;
			else
				Destroy(gameObject);
			
			if (Platforms == null)
				Debug.LogError("No PlatformManager attached on " + this + "!");
			if (Score == null)
				Debug.LogError("No GameScore attached on " + this + "!");
			
			Time.timeScale = 1f;
		}

		private void OnEnable()
		{
			Player.Instance.Controls.InGame.Exit.performed += OnExitPerformed;
			Player.Instance.PlayerDeath += OnPlayerDeath;
		}

		private void OnDisable()
		{
			Player.Instance.Controls.InGame.Exit.performed -= OnExitPerformed;
			Player.Instance.PlayerDeath -= OnPlayerDeath;
		}

		private void OnPlayerDeath() => StartCoroutine(DeathCoroutine());

		private IEnumerator DeathCoroutine()
		{
			Time.timeScale = .5f;
			yield return imageFader.FadeTo(1f, .5f);
			Time.timeScale = 0f;
			yield return new WaitForSecondsRealtime(0.2f);
			Time.timeScale = 1f;
			SceneManager.LoadScene(0);
		}

		// Return to main menu.
		private static void OnExitPerformed(InputAction.CallbackContext _) => SceneManager.LoadScene(0);
	}
}
