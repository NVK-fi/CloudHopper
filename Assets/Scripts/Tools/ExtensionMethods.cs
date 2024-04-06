namespace Tools
{
	using UnityEngine;
    using System;
	
	public static class ExtensionMethods
	{
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null) 
		    => new(x ?? original.x, y ?? original.y, z ?? original.z);
		
		public static Color With(this Color original, float? r = null, float? g = null, float? b = null, float? a = null) 
			=> new(r ?? original.r, g ?? original.g, b ?? original.b, a ?? original.a);

		public static bool IsBetween(this float number, float lower, float upper) 
	        => number > lower && number < upper;

		public static Vector3 ClampUniform(this Vector3 vector3, float min, float max)
		    => new Vector3(
				Mathf.Clamp(vector3.x, min, max),
				Mathf.Clamp(vector3.y, min, max),
				Mathf.Clamp(vector3.z, min, max));
		
		public static T PickRandom<T>(this T[] array)
		{
            if (array == null || array.Length == 0)
				throw new ArgumentException("Array is null or empty.", nameof(array));

			var index = new System.Random().Next(array.Length);
			return array[index];
		}
	}
}