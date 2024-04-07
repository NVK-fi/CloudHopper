using UnityEngine;

namespace Animation
{
	/// <summary>
	/// Moves the attached Transform up and down locally using sine wave.
	/// </summary>
	public class MoveUpAndDown : MonoBehaviour
	{
		[SerializeField] private float radius = 1f;
		[SerializeField] private float cycleDuration = 1f;

		private float _elapsedTime;
		private Vector3 _lastOffset = Vector3.zero;

		private void Update()
		{
			_elapsedTime += Time.deltaTime;
			_elapsedTime %= cycleDuration;

			// This value cycles between -1f and 1f.
			var normalizedDistance = Mathf.Sin(2 * Mathf.PI * _elapsedTime / cycleDuration);

			// Calculate the change of position and apply it.
			var currentOffset = Vector3.up * (normalizedDistance * radius);
			var positionChange = currentOffset - _lastOffset;
			transform.localPosition += positionChange;

			_lastOffset = currentOffset;
		}
	}
}
