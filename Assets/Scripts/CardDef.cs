using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor;

public class CardDef:MonoBehaviour {
	public enum Gender { Male, Female }
	public enum Conservation { LeastConcern, NearThreatened, Vulnerable, Endangered, CriticallyEndangered }
	public GameObject meshPrefab;
	public RuntimeAnimatorController animatorController;
	public Material material;
	public float animalScaler = 1;
	public string displayName;
	public Gender gender = Gender.Male;
	public Conservation conservation = Conservation.LeastConcern;
	public Texture2D infoMapTexture;
	public int maxCount;
    public AudioClip spokenName;
	public bool neverSkip;
	public float probability
	{
		get
		{
			return GetProbability(conservation);
		}
	}

	private static float[] probabilityLookup = new float[]
	{
		0.95f,
		0.25f,
		0.5f,
		0.75f,
		0.05f,
	};
	public static float GetProbability(Conservation conservation)
	{
		return probabilityLookup[(int)conservation];
	}
}
