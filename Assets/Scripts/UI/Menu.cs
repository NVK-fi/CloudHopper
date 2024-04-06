using UnityEngine;

namespace UI
{
	using System;
	using System.Collections;
	using Controls;
	using Settings;
	using TMPro;
	using Tools;
	using UnityEngine.InputSystem;
	using UnityEngine.SceneManagement;

	/// <summary>
	/// Represents the main menu.
	/// </summary>
	[RequireComponent(typeof(CanvasGroup))]
	public class Menu : MonoBehaviour
	{
		[SerializeField] private CanvasGroupFader canvasGroupFader;
		[SerializeField] private ImageFader overlayFader;
		[SerializeField] private TextMeshProUGUI lastScoreTextContainer;
		[SerializeField] private TextMeshProUGUI highScoreTextContainer;
		[SerializeField] private TextMeshProUGUI startButtonTextContainer;
		[SerializeField] private string[] startButtonTexts;
		[SerializeField] private ParticleSystem rainParticles;

		private CanvasGroup _canvasGroup;
		private InputAsset _controls;
		
		private void Awake()
		{
			if (lastScoreTextContainer == null || highScoreTextContainer == null || startButtonTextContainer == null)
			{
				Debug.LogError("No text container set on " + this + "!");
				enabled = false;
			}

			if (overlayFader == null || canvasGroupFader == null)
			{
				Debug.LogError("No fader set on " + this + "!");
				enabled = false;
			}

			_canvasGroup = GetComponent<CanvasGroup>();
			_controls = new InputAsset();
		}

		private void OnEnable()
		{
			_controls.Enable();
			_controls.UI.Cancel.performed += ExitGame;
		}

		private void OnDisable() => _controls.Disable();

		private void Start()
		{
			var lastScore = PlayerPrefs.GetInt(Constants.LAST_SCORE_KEY, 0); 
			lastScoreTextContainer.text = "Last Score: " + lastScore;
			
			var highScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
			highScoreTextContainer.text = "High Score: " + highScore;
			
			if (startButtonTexts.Length > 0)
				startButtonTextContainer.text = startButtonTexts.PickRandom();

			StartCoroutine(MenuAppearingCoroutine());
		}

		private IEnumerator MenuAppearingCoroutine()
		{
			yield return overlayFader.FadeTo(0, .1f);
			
			_canvasGroup.interactable = true;
			yield return canvasGroupFader.FadeTo(1, .5f);
		}

		public void StartGame() => StartCoroutine(GameStartCoroutine());

		private IEnumerator GameStartCoroutine()
		{
			var main = rainParticles.main;
			main.startLifetime = 0.1f;
			rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			
			canvasGroupFader.StopAllCoroutines();
			yield return canvasGroupFader.FadeTo(0, .3f);
			SceneManager.LoadScene(1);
		}

		private static void ExitGame(InputAction.CallbackContext _)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}
