using System;
using System.Collections;
using UnityEngine;

namespace Platforms
{
	[RequireComponent(typeof(Collider))]
	public class Platform : MonoBehaviour
	{
		private Vector3 _initialScale;
		private Collider _collider;
		
		private void Awake()
		{
			_collider = GetComponent<Collider>();
			_initialScale = transform.localScale;
		}
	}
}