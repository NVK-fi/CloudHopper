namespace Tools
{
	using UnityEngine;
    using System;
	using Settings;

	/// <summary>
	/// Contains extension methods for various types.
	/// </summary>
	public static class ExtensionMethods
	{
		/// <summary>
		/// Returns a new Vector3 with specified components.
		/// If a component is not specified, the original component value will be used.
		/// </summary>
		/// <returns>A new Vector3 with the specified components.</returns>
		public static Vector3 With(this Vector3 original, float? x = null, float? y = null, float? z = null)
			=> new(x ?? original.x, y ?? original.y, z ?? original.z);

		/// <summary>
		/// An extension method that returns a new Color with specified components.
		/// If a component is not specified, the original component value will be used.
		/// </summary>
		/// <returns>A new Color with the specified components.</returns>
		public static Color With(this Color original, float? r = null, float? g = null, float? b = null,
			float? a = null)
			=> new(r ?? original.r, g ?? original.g, b ?? original.b, a ?? original.a);

		/// <summary>
		/// Determines whether the specified number is between the given lower and upper values (exclusive).
		/// </summary>
		/// <returns>true if the number is between the lower and upper bounds; otherwise, false.</returns>
		public static bool IsBetween(this float number, float lower, float upper)
			=> number > lower && number < upper;

		/// <summary>
		/// Clamps each of the components of a Vector3 to a given range.
		/// </summary>
		/// <returns>A new Vector3 with clamped components.</returns>
		public static Vector3 ClampUniform(this Vector3 vector3, float min, float max)
			=> new(
				Mathf.Clamp(vector3.x, min, max),
				Mathf.Clamp(vector3.y, min, max),
				Mathf.Clamp(vector3.z, min, max));

		/// <summary>
		/// Picks a random element from the given array.
		/// The array can not be null or empty.
		/// </summary>
		/// <returns>A random element from the array.</returns>
		public static T PickRandom<T>(this T[] array)
		{
			if (array == null || array.Length == 0)
				throw new ArgumentException("Array is null or empty.", nameof(array));

			var index = new System.Random().Next(array.Length);
			return array[index];
		}
	}
}