using UnityEngine;

namespace UI
{
	using System.Collections;
	using Controls;
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
		[SerializeField] private ParticleSystem rainParticles;

		private CanvasGroup _canvasGroup;
		private InputAsset _controls;
		
		private void Awake()
		{
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

		private void OnDisable()
		{
			_controls.Disable();
			_controls.UI.Cancel.performed -= ExitGame;
		}

		private void Start() => StartCoroutine(MenuAppearingTransition());

		private IEnumerator MenuAppearingTransition()
		{
			Time.timeScale = 1f;
			
			yield return overlayFader.FadeTo(0, .1f);
			
			_canvasGroup.interactable = true;
			yield return canvasGroupFader.FadeTo(1, .5f);
		}

		public void PressStart() => StartCoroutine(GameStartTransition());

		private IEnumerator GameStartTransition()
		{
			rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

			_canvasGroup.interactable = false;
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
