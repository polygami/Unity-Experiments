using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static cMath;

public class Atom : MonoBehaviour
{
	[SerializeField] public int Protons = 1;
	[SerializeField] public int Neutrons = 0;
	[SerializeField] public GameObject Model;
	[SerializeField] public GameObject Shell;
	[SerializeField] public Material ProtonMaterial;
	[SerializeField] public Material NeutronMaterial;
	[SerializeField] public float MaxRotation;
	[SerializeField] public float NucleusRadius = 1;
	[SerializeField] public float NucleonRelativeDistance = 1;
	[SerializeField] public bool IsEveryNucleonScaled = true;
	[SerializeField] public bool IsNucleusScaled = false;
	private int _nucleons;
	public int Nucleons
	{
		get { return _nucleons; }
	}

	Vector3 randomRotation;

	// Start is called before the first frame update
	void Start()
	{
		_nucleons = Protons + Neutrons;
		InstantiateNucleons();
		InstantiateElectrons();
		randomRotation = Random.rotation.eulerAngles * MaxRotation;
	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(randomRotation * Time.deltaTime);
	}

	void InstantiateNucleons()
	{
		// Get Points
		Vector3[] points = cMath.SphericalFibonacciLattice(_nucleons);
		// Create array of TRUE based on the amount of protons
		bool[] isProtonArray = GetIsProtonArray();

		float π4 = Mathf.PI * 4f;
		// float v = Nucleons * NucleusRadius;
		float v = Nucleons;
		float volumeToSurfaceArea = Mathf.Sqrt(v / π4);
		// float test = volumeToSurfaceArea * NucleusRadius / Nucleons;
		float scaleMultiplier = volumeToSurfaceArea / NucleusRadius;
		Vector3 nucleonScale = IsEveryNucleonScaled ? Vector3.one / volumeToSurfaceArea * NucleusRadius : Vector3.one * NucleusRadius;
		float nucleusScale = IsEveryNucleonScaled ? NucleusRadius : volumeToSurfaceArea * NucleusRadius;
		// float multiplier = volumeToSurfaceArea * test;

		for (int i = 0; i < points.Length; i++)
		{
			Vector3 point = points[i];
			// Instantiate and parent to atom
			GameObject obj = Instantiate(Model, point * nucleusScale, new Quaternion(), gameObject.transform);
			obj.transform.localScale = nucleonScale;
			//Apply Material
			obj.GetComponent<Renderer>().material = isProtonArray[i] ? ProtonMaterial : NeutronMaterial;
		}
	}

	bool[] GetIsProtonArray()
	{
		// Create array of TRUE based on the amount of protons
		bool[] areProtons = new bool[Protons];
		System.Array.Fill<bool>(areProtons, true);
		// Create array of FALSE based on the amount of neutrons
		bool[] areNotProtons = new bool[Neutrons];
		System.Array.Fill<bool>(areNotProtons, false);
		// Merge the arrays
		bool[] mergedArray = new bool[Protons + Neutrons];
		areProtons.CopyTo(mergedArray, 0);
		areNotProtons.CopyTo(mergedArray, areProtons.Length);
		// Randomize the array
		System.Random r = new System.Random();
		return mergedArray.OrderBy(x => r.Next()).ToArray();
	}

	void InstantiateElectrons()
	{
		int[] distribution = ElectronDistribution(Protons);
		string arrayAsString = string.Join(", ", distribution);
		Debug.Log("Proton count: " + Protons);
		Debug.Log("Electron distribution: " + arrayAsString);

		for (int shell = 0; shell < distribution.Length; shell++)
		{
			GameObject shellInstance = Instantiate(Shell, transform);
			shellInstance.GetComponent<Shell>().Initialize(3 + shell, distribution[shell]);
		}
		// Debug.Log("Electron distribution: " + )ElectronDistribution(Protons).Join<();
	}

	int[] ElectronDistribution(int electronCount)
	{
		int shellCount = 0;
		int remainingElectrons = electronCount;
		List<int> electronDistribution = new List<int>();

		while (remainingElectrons != 0 && shellCount < 10)
		{
			shellCount++;
			int electronsInCurrentShell = Mathf.Min(remainingElectrons, GetMaxElectronsInShell(shellCount));
			remainingElectrons -= electronsInCurrentShell;
			electronDistribution.Add(electronsInCurrentShell);
		}
		return electronDistribution.ToArray();
	}

	int GetMaxElectronsInShell(int shellNumber)
	{
		// 2n^2
		return 2 * shellNumber * shellNumber;
	}

}
