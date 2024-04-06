using UnityEngine;

namespace Tools
{
	public class LimitFPS : MonoBehaviour
	{
		private void Start() => Application.targetFrameRate = 300;
	}
}
