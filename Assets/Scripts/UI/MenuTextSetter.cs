using UnityEngine;

namespace UI
{
	using Settings;
	using TMPro;
	using Tools;

	/// <summary>
	/// This class is responsible for setting the initial texts of main menu UI items.
	/// </summary>
	public class MenuTextSetter : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI lastScoreTextContainer;
		[SerializeField] private TextMeshProUGUI highScoreTextContainer;
		[SerializeField] private TextMeshProUGUI startButtonTextContainer;
		[SerializeField] private string[] startButtonTexts;

		private void Awake()
		{
			if (lastScoreTextContainer == null || highScoreTextContainer == null || startButtonTextContainer == null)
			{
				Debug.LogError("No text container set on " + this + "!");
				enabled = false;
			}
		}

		private void Start()
		{
			var lastScore = PlayerPrefs.GetInt(Constants.LAST_SCORE_KEY, 0); 
			lastScoreTextContainer.text = "Last Score: " + lastScore;
			
			var highScore = PlayerPrefs.GetInt(Constants.HIGH_SCORE_KEY, 0);
			highScoreTextContainer.text = "High Score: " + highScore;
			
			if (startButtonTexts.Length > 0)
				startButtonTextContainer.text = startButtonTexts.PickRandom();
		}
	}
}
