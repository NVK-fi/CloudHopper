using UnityEngine;

namespace Game
{
	using System.Collections;
	using UI;
	using UnityEngine.InputSystem;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Controls scene transitions from game to menu scene.
	/// </summary>
	[RequireComponent(typeof(Game))]
	public class GameToMenuTransition : MonoBehaviour
	{
		[SerializeField] private float deathExitDuration = .75f;
		[SerializeField] private float manualExitDuration = .25f;
		[SerializeField] private ImageFader imageFader;
		
		private void OnEnable()
		{
			Game.Instance.Player.Death += OnPlayerDeath;
			Game.Instance.Controls.InGame.Exit.performed += OnExitPerformed;
		}

		private void OnDisable()
		{
			Game.Instance.Player.Death -= OnPlayerDeath;
			Game.Instance.Controls.InGame.Exit.performed -= OnExitPerformed;
		}

		private void OnPlayerDeath() => StartCoroutine(FadeToMenu(deathExitDuration));

		// This is called when the player presses escape.
		private void OnExitPerformed(InputAction.CallbackContext _) => StartCoroutine(FadeToMenu(manualExitDuration));
		
		private IEnumerator FadeToMenu(float duration)
		{
			Time.timeScale = .5f;
			yield return imageFader.FadeTo(1f, duration * .75f);
			Time.timeScale = 0f;
			yield return new WaitForSecondsRealtime(duration * .25f);
			Time.timeScale = 1f;
			SceneManager.LoadScene(0);
		}
	}
}
