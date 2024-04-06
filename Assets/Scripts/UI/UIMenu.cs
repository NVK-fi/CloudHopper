using UnityEngine;

namespace UI
{
	using System.Collections;
	using Settings;
	using TMPro;
	using Tools;
	using UnityEngine.SceneManagement;

	[RequireComponent(typeof(CanvasGroup))]
	public class UIMenu : MonoBehaviour
	{
		[SerializeField] private UICanvasFader canvasFader;
		[SerializeField] private UIImageFader overlayFader;
		[SerializeField] private TextMeshProUGUI lastScoreTextContainer;
		[SerializeField] private TextMeshProUGUI highScoreTextContainer;
		[SerializeField] private TextMeshProUGUI startButtonTextContainer;
		[SerializeField] private string[] startButtonTexts;
		[SerializeField] private ParticleSystem rainParticles;

		private CanvasGroup _canvasGroup;
		
		private void Awake()
		{
			if (lastScoreTextContainer == null || highScoreTextContainer == null || startButtonTextContainer == null)
			{
				Debug.LogError("No text container set on " + this + "!");
				enabled = false;
			}

			if (overlayFader == null || canvasFader == null)
			{
				Debug.LogError("No fader set on " + this + "!");
				enabled = false;
			}

			_canvasGroup = GetComponent<CanvasGroup>();
		}
		
		private void Start()
		{
			var lastScore = PlayerPrefs.GetInt(SettingsStrings.LastScore, 0); 
			lastScoreTextContainer.text = "Last Score: " + lastScore;
			
			var highScore = PlayerPrefs.GetInt(SettingsStrings.HighScore, 0);
			highScoreTextContainer.text = "High Score: " + highScore;
			
			if (startButtonTexts.Length > 0)
				startButtonTextContainer.text = startButtonTexts.PickRandom();

			StartCoroutine(MenuAppearingCoroutine());
		}

		private IEnumerator MenuAppearingCoroutine()
		{
			yield return overlayFader.FadeTo(0, .1f);
			
			_canvasGroup.interactable = true;
			yield return canvasFader.FadeTo(1, .5f);
		}

		public void StartGame() => StartCoroutine(GameStartCoroutine());

		private IEnumerator GameStartCoroutine()
		{
			var main = rainParticles.main;
			main.startLifetime = 0.1f;
			rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			
			canvasFader.StopAllCoroutines();
			yield return canvasFader.FadeTo(0, .3f);
			SceneManager.LoadScene(1);
		}
	}
}
