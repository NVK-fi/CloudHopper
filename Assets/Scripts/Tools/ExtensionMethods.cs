namespace Tools
{
	using UnityEngine;

	public static class ExtensionMethods
	{
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) 
			=> new(x ?? original.x, y ?? original.y, z ?? original.z);

		public static bool IsBetween(this float number, float lower, float upper) 
	        => number > lower && number < upper;
	}
}