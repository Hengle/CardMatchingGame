using UnityEngine;
using System.Collections;

public class Background:MonoBehaviour
{
	public Transform hyenaLocation;
	public Hyena hyenaPrefab;
	public Hyena hyena { get; private set; }

	void Awake()
	{
		hyena = Instantiate(hyenaPrefab);
		hyena.transform.SetParent(hyenaLocation, false);
	}
}
