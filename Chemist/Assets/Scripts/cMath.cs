using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class cMath
{
	// public static const 
	// C# Code example found at https://stackoverflow.com/questions/34142701/arrange-x-amount-of-things-evenly-around-a-point-in-3d-space/69750917#69750917
	private static float sphereVolumeMultiplier = Mathf.PI * 4 / 3;

	public static Vector3[] SphericalFibonacciLattice(int n)
	{
		Vector3[] res = new Vector3[n];
		float goldenRatio = (1.0f + Mathf.Sqrt(5.0f)) * 0.5f;
		for (int i = 0; i < n; i++)
		{
			float theta = 2.0f * Mathf.PI * i / goldenRatio;
			float phi = Mathf.Acos(1.0f - 2.0f * (i + 0.5f) / n);
			Vector3 p = new Vector3(Mathf.Cos(theta) * Mathf.Sin(phi),
									Mathf.Sin(theta) * Mathf.Sin(phi),
									Mathf.Cos(phi));
			res[i] = p;
		}
		return res;
	}

	public static int GetMaxElectronsInShell(int shellNumber)
	{
		// 2n^2
		return 2 * shellNumber * shellNumber;
	}

	public static float GetSphereVolume(float radius)
	{
		return sphereVolumeMultiplier * Mathf.Pow(radius, 3);
	}
}