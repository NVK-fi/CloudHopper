using UnityEngine;

namespace UI
{
	using System;
	using Settings;
	using TMPro;
	using UnityEngine.UI;

	[RequireComponent(typeof(Scrollbar))]
	public class UIMenuSensitivitySlider : MonoBehaviour
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
			var sensitivity = PlayerPrefs.GetInt(SettingsStrings.Sensitivity, 5);
			_scrollbar.value = sensitivity * .1f;
		}

		public void OnValueChanged()
		{
			var value = Mathf.RoundToInt(_scrollbar.value * 10);
			handle.text = "[" + value + "]";
			
			PlayerPrefs.SetInt(SettingsStrings.Sensitivity, value);
			PlayerPrefs.Save();
		}
	}
}
