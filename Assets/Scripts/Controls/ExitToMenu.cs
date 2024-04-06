namespace Controls
{
	using Player;
	using UnityEngine;
	using UnityEngine.InputSystem;
	using UnityEngine.SceneManagement;

	public class ExitToMenu : MonoBehaviour
	{
		private void OnEnable() => Player.Instance.Controls.InGame.Exit.performed += OnExit;
		private void OnDisable() => Player.Instance.Controls.InGame.Exit.performed -= OnExit;

		private void OnExit(InputAction.CallbackContext _) => SceneManager.LoadScene(0);
	}
}
