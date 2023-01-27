using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
	public int Sides;
	// public float Radius;
	public float Width;
	public int OverdrawCount;
	public LineRenderer Line;

	public float maxRotation;
	Vector3 rotation;

	[SerializeField] GameObject Electron;
	// Start is called before the first frame update
	void Start()
	{

		transform.rotation = Random.rotation;
		Vector3 randomVector = Random.rotation.eulerAngles;
		rotation = new Vector3(randomVector.x / 10f, Random.value * maxRotation, randomVector.z / 10f);


		// rotation = Random.rotation.eulerAngles * maxRotation / 360f;
	}

	// Update is called once per frame
	void Update()
	{


		transform.Rotate(rotation * Time.deltaTime);
	}

	public void Initialize(float Radius, int ElectronCount)
	{
		Line.positionCount = Sides + OverdrawCount;
		float tau = 2 * Mathf.PI;

		for (int point = 0; point < Line.positionCount; point++)
		{
			float radian = ((float)point / Sides) % Sides * tau;
			float x = Mathf.Cos(radian) * Radius;
			float y = Mathf.Sin(radian) * Radius;
			Line.SetPosition(point, new Vector3(x, 0, y));
		}
		for (int electron = 0; electron < ElectronCount; electron++)
		{
			float radian = ((float)electron / ElectronCount) * tau;
			float x = Mathf.Cos(radian) * Radius;
			float y = Mathf.Sin(radian) * Radius;
			Instantiate(Electron, new Vector3(x, 0, y), new Quaternion(), transform);
		}
		Line.loop = true;
		Line.widthMultiplier = Width;

	}
}
