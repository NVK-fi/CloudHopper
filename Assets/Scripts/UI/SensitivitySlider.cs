using UnityEngine;

namespace UI
{
	using Settings;
	using TMPro;
	using UnityEngine.UI;

	/// <summary>
	/// A slider that adjusts look sensitivity setting.
	/// </summary>
	[RequireComponent(typeof(Scrollbar))]
	public class SensitivitySlider : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI handle;
		private Scrollbar _scrollbar;
		
		private void Awake()
		{
			_scrollbar = GetComponent<Scrollbar>();
			
			if (handle == null)
			{
				Debug.LogError("No handle reference attached on " + this + "!");
				enabled = false;
			}
		}

		private void Start()
		{
			var sensitivity = PlayerPrefs.GetInt(Constants.SENSITIVITY_KEY, 5);
			_scrollbar.value = sensitivity * .1f;
		}

		public void OnValueChanged()
		{
			var value = Mathf.RoundToInt(_scrollbar.value * 10);
			handle.text = "[" + value + "]";
			
			PlayerPrefs.SetInt(Constants.SENSITIVITY_KEY, value);
			PlayerPrefs.Save();
		}
	}
}
