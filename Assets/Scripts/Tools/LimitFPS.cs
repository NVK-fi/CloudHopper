using UnityEngine;

namespace Tools
{
	/// <summary>
	/// A class that limits the frames per second (FPS) of a game.
	/// </summary>
	public class LimitFPS : MonoBehaviour
	{
		private void Start() => Application.targetFrameRate = 300;
	}
}
