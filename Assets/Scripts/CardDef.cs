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
}
