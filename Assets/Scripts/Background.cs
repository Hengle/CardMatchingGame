using UnityEngine;
using System.Collections;

public class Background:MonoBehaviour
{
	public TimeOfDay timeOfDay;
	public Transform hyenaLocation;
	public Hyena hyenaPrefab;
	public Hyena hyena { get; private set; }

	void Awake()
	{
		hyena = Instantiate(hyenaPrefab);
		hyena.transform.SetParent(hyenaLocation, false);
		
	}

	private void Start()
	{
		SetLayerRecursive(gameObject);
	}

	private void SetLayerRecursive(GameObject go)
	{
		go.layer = LayerMask.NameToLayer("Background");
		for (int i = 0; i < go.transform.childCount; i++)
		{
			SetLayerRecursive(go.transform.GetChild(i).gameObject);
		}
	}
}
