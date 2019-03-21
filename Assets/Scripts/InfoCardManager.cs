using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoCardManager : MonoBehaviour
{
    public GameObject goLeftArrow;
    public GameObject goRightArrow;

    public CardDef[] goUnlocked;
    public GameObject[] goSlots;
    public GameObject goBackground;
    public GameObject goMenuBackground;
    public GameObject goBackButton;
	public Button backButton;
	public Button factSheetBackButton;

	public Text animalNameText;
	public Text nameText;
    public RawImage animalGender;
    public Text animalConservationText;
    public RawImage animalMap;
    public Text animalPercentage;
    public AudioSource antilopeAS;

	public Texture2D genderTextureMale;
	public Texture2D genderTextureFemale;

    public Texture2D[] backgroundInfoTextures;
    private int currentPlace = 0;
    private int currentInfoBackground = 0;
    private bool isFlipping = false;
    private bool isEnd = false;
    private GameObject newMaleMesh;
    private GameObject newFemaleMesh;
    private bool animationEnd = true;
    private AudioClip nameAudio;
    public AudioClip[] uiAC;

    public void AntilopeClicked()
    {
        newMaleMesh.GetComponent<Animator>().SetTrigger("AudioDance");
        newFemaleMesh.GetComponent<Animator>().SetTrigger("AudioDance");

        if (!antilopeAS.isPlaying)
        {
            antilopeAS.clip = nameAudio;
            antilopeAS.Play();
        }
    }

    private void Awake()
    {
        FillSlots();
    }

    private void OnEnable()
    {
		ResetState();
        FillSlots();
    }

    private void OnDisable()
    {
		ResetState();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFlipping)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.gameObject.name == goLeftArrow.name)
                {
                    Arrow("Left");
                    currentPlace--;
                }
                else if(hit.collider.gameObject.name == goRightArrow.name)
                {
					Arrow("Right");
                    currentPlace++;
                }
            }
        }

		/*
        if(!goBackButton.GetComponent<Info>().factSheet)
        {
            if (animationEnd && newMaleMesh && newFemaleMesh)
            {
                
            }
        }
		*/
    }
    
	public void BackFromInfoSheet()
	{
		factSheetBackButton.gameObject.SetActive(false);

		animationEnd = false;
		newMaleMesh.GetComponent<Animator>().SetTrigger("Run");
		StartCoroutine(AnimateWalkToAndAway(newMaleMesh, true));

		newFemaleMesh.GetComponent<Animator>().SetTrigger("Run");
		StartCoroutine(AnimateWalkToAndAway(newFemaleMesh, true));
	}

    public void ShowFactSheet(InfoCards animal)
    {
        //goBackButton.GetComponent<Info>().factSheet = true;
        nameText.gameObject.SetActive(false);
        foreach(GameObject slot in goSlots)
        {
            slot.SetActive(false);
        }

		backButton.gameObject.SetActive(false);
		
		//goBackButton.SetActive(false);
		goMenuBackground.SetActive(false);
        goBackground.SetActive(true);
        goLeftArrow.SetActive(false);
        goRightArrow.SetActive(false);

        var animalMaleSlot = goBackground.transform.Find("animal Male");

        if (animalMaleSlot.Find("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalMaleSlot.Find("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);

        backgroundInfoTextures = animal.infoTextures;

		float maleScale = animal.goUnlocked.animalScaler;
		float femaleScale = animal.goUnlocked.femaleEquivalent.GetComponent<CardDef>().animalScaler;

		float maxScale = Math.Max(maleScale, femaleScale);
		maleScale /= maxScale;
		femaleScale /= maxScale;

		FillFactSheet(animal.goUnlocked);
		newMaleMesh = Fill(animalMaleSlot.gameObject, animal, maleScale);

        animalMaleSlot.gameObject.SetActive(true);
        animalMaleSlot.GetComponent<Animator>().Play("FlipToFront");
        newMaleMesh.GetComponent<Animator>().SetTrigger("Run");
        StartCoroutine(AnimateWalkToAndAway(newMaleMesh, false));

        var animalFemaleSlot = goBackground.transform.Find("animal Female");

        if (animalFemaleSlot.Find("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalFemaleSlot.Find("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);

        FillFactSheet(animal.goUnlocked);
        newFemaleMesh = FillFemale(animalFemaleSlot.gameObject, animal, femaleScale);

        animalFemaleSlot.gameObject.SetActive(true);
        animalFemaleSlot.GetComponent<Animator>().Play("FlipToFront");
        newFemaleMesh.GetComponent<Animator>().SetTrigger("Run");
        StartCoroutine(AnimateWalkToAndAway(newFemaleMesh, false));

        //Flip(animalSlot.GetComponent<Animator>(), true);
    }

    public void FlipInfoBackgroundChange()
    {
        currentInfoBackground++;

        if (currentInfoBackground > backgroundInfoTextures.Length - 1)
            currentInfoBackground = 0;

        animalMap.texture = backgroundInfoTextures[currentInfoBackground];
    }

    private IEnumerator AnimateWalkToAndAway(GameObject animalPivot, bool away)
    {
        yield return new WaitForSeconds(0.1f);
        float counter = 0;

        if (away)
        {
			//goBackButton.SetActive(false);
            goMenuBackground.SetActive(false);
            while (counter < 2)
            {
                counter += Time.deltaTime;
                float ratio = counter / 2;
                animalPivot.transform.localPosition = new Vector3(Mathf.Lerp(2.5f, 6, ratio), 0, Mathf.Lerp(0, 0.225f, ratio * 5));
                yield return 0;
            }
            Destroy(animalPivot.gameObject);

            foreach (GameObject slot in goSlots)
            {
                slot.SetActive(true);
            }
			//backButton.gameObject.SetActive(true);
			//goBackButton.SetActive(true);
            goBackground.SetActive(false);
            goLeftArrow.SetActive(true);
            goRightArrow.SetActive(true);
            animationEnd = true;
        }
        else
        {
            while (counter < 2)
            {
                counter += Time.deltaTime;
                float ratio = counter / 2;
                animalPivot.transform.localPosition = new Vector3(Mathf.Lerp(-2, 2.5f, ratio), 0, Mathf.Lerp(0, 0.225f, ratio * 5));
                yield return 0;
            }
			backButton.gameObject.SetActive(false);
			//goBackButton.SetActive(true);
            goMenuBackground.SetActive(true);
            animalPivot.GetComponent<Animator>().SetTrigger("Idle");
        }

		backButton.gameObject.SetActive(away);
		factSheetBackButton.gameObject.SetActive(!away);
	}

    Dictionary<CardDef.Conservation, string> conservationTextDict = new Dictionary<CardDef.Conservation, string>()
	{
		{ CardDef.Conservation.LeastConcern, "Least Concern" },
		{ CardDef.Conservation.NearThreatened, "Near Threatened" },
		{ CardDef.Conservation.Vulnerable, "Vulnerable" },
		{ CardDef.Conservation.Endangered, "Endangered" },
		{ CardDef.Conservation.CriticallyEndangered, "Critically Endangered" }
	};
	private void FillFactSheet(CardDef cardDef)
    {
        string name = cardDef.displayName;
		if (string.IsNullOrEmpty(name))
		{
			name = cardDef.name;
		}
		animalNameText.text = name;
		// animalGender.texture = cardDef.gender == CardDef.Gender.Male ? genderTextureMale : genderTextureFemale;
        animalConservationText.text = conservationTextDict[cardDef.conservation];
        currentInfoBackground = 0;
        animalMap.texture = backgroundInfoTextures[0];
    }

    private void Arrow(string direction)
    {
        isFlipping = true;
        nameText.gameObject.SetActive(false);
        Flip(false);
        OneShotAudio.Play(uiAC[UnityEngine.Random.Range(0, uiAC.Length)], 0, GameSettings.Audio.sfxVolume);
        StartCoroutine(WaitFillSlots(0.5f));
    }

    private IEnumerator WaitFillSlots(float sec)
    {
        yield return new WaitForSeconds(sec);
        FillSlots();
    }

    private  void FillSlots()
    {
        if (currentPlace >= goUnlocked.Length)
            currentPlace = 0;
        
        if (currentPlace < 0)
            currentPlace = goUnlocked.Length - 1;

        int slot1Pos = currentPlace - 1;
        int slot2Pos = currentPlace;
        int slot3Pos = currentPlace + 1;

        if(slot1Pos >= goUnlocked.Length)
            slot1Pos = 0;

        if (slot1Pos < 0)
            slot1Pos = goUnlocked.Length - 1;

        if(slot2Pos >= goUnlocked.Length)
            slot2Pos = 0;

        if (slot2Pos < 0)
            slot2Pos = goUnlocked.Length - 1;

        if(slot3Pos >= goUnlocked.Length)
            slot3Pos = 0;

        if (slot3Pos < 0)
            slot3Pos = goUnlocked.Length - 1;

        // Debug.Log("CurrentPlace " + currentPlace);
        // Debug.Log("Slot 1 is == to " + slot1Pos);
        // Debug.Log("Slot 2 is == to " + slot2Pos);
        // Debug.Log("Slot 3 is == to " + slot3Pos);

        InfoCards ic = goSlots[0].GetComponent<InfoCards>();
        Transform animalPivot = ic.gameObject.transform.Find("RotationPivot/AnimalPivot");

        InfoCards ic1 = goSlots[1].GetComponent<InfoCards>();
        Transform animalPivot1 = ic1.gameObject.transform.Find("RotationPivot/AnimalPivot");

        InfoCards ic2 = goSlots[2].GetComponent<InfoCards>();
        Transform animalPivot2 = ic2.gameObject.transform.Find("RotationPivot/AnimalPivot");

        while (animalPivot.childCount > 0) 
        {
            DestroyImmediate (animalPivot.GetChild(0).gameObject);
        }

        while (animalPivot1.childCount > 0) 
        {
            DestroyImmediate (animalPivot1.GetChild(0).gameObject);
        }

        while (animalPivot2.childCount > 0) 
        {
            DestroyImmediate (animalPivot2.GetChild(0).gameObject);
        }

        ic.goUnlocked = goUnlocked[slot1Pos];
        ic.animatorController = goUnlocked[slot1Pos].animatorController;
        ic.spokenName = goUnlocked[slot1Pos].spokenName;
        ic.gender = goUnlocked[slot1Pos].gender.ToString();
        ic.infoTextures = goUnlocked[slot1Pos].infoTextures;
        ic.displayName = goUnlocked[slot1Pos].displayName;

        ic1.goUnlocked = goUnlocked[slot2Pos];
        ic1.animatorController = goUnlocked[slot2Pos].animatorController;
        ic1.spokenName = goUnlocked[slot2Pos].spokenName;
        ic1.gender = goUnlocked[slot2Pos].gender.ToString();
        ic1.infoTextures = goUnlocked[slot2Pos].infoTextures;
        ic1.displayName = goUnlocked[slot2Pos].displayName;
        nameText.text = ic1.displayName;

        ic2.goUnlocked = goUnlocked[slot3Pos];
        ic2.animatorController = goUnlocked[slot3Pos].animatorController;
        ic2.spokenName = goUnlocked[slot3Pos].spokenName;
        ic2.gender = goUnlocked[slot3Pos].gender.ToString();
        ic2.infoTextures = goUnlocked[slot3Pos].infoTextures;
        ic2.displayName = goUnlocked[slot3Pos].displayName;

        Fill(ic.gameObject, ic);
        Fill(ic1.gameObject, ic1);
        Fill(ic2.gameObject, ic2);

        if (!PlayerPrefs.HasKey(ic.goUnlocked.ToString()))
        {
            StartCoroutine(CardFlip(goSlots[0].GetComponent<Animator>(), true));
        }

        if (!PlayerPrefs.HasKey(ic1.goUnlocked.ToString()))
        {
            StartCoroutine(CardFlip(goSlots[1].GetComponent<Animator>(), true));
        }

        if (!PlayerPrefs.HasKey(ic2.goUnlocked.ToString()))
        {
            StartCoroutine(CardFlip(goSlots[2].GetComponent<Animator>(), true));
        }

        // currentPlace++;
        // foreach (GameObject slot in goSlots)
        // {
        //     InfoCards ic = slot.GetComponent<InfoCards>();

		// 	Transform animalPivot = ic.gameObject.transform.Find("RotationPivot/AnimalPivot");
		// 	while (animalPivot.childCount > 0) {
		// 		DestroyImmediate (animalPivot.GetChild(0).gameObject);
		// 	}

        //     if (currentPlace >= goUnlocked.Length)
        //     {
        //         isEnd = true;
        //         continue;
        //     }

        //     if (currentPlace < 0)
        //         currentPlace = goUnlocked.Length - 1;

        //     ic.goUnlocked = goUnlocked[currentPlace];
        //     ic.animatorController = goUnlocked[currentPlace].animatorController;
        //     ic.spokenName = goUnlocked[currentPlace].spokenName;
        //     ic.gender = goUnlocked[currentPlace].gender.ToString();
        //     ic.infoTextures = goUnlocked[currentPlace].infoTextures;

        //     Fill(ic.gameObject, ic);

        //     if (!PlayerPrefs.HasKey(ic.goUnlocked.ToString()))
        //     {
        //         StartCoroutine(CardFlip(slot.GetComponent<Animator>(), true));
        //     }

        //     currentPlace++;
        // }
        isFlipping = false;
    }

    private GameObject Fill(GameObject slot, InfoCards ic, float scale = 1)
    {
        //slot.name = ic.goUnlocked.ToString();

		Transform animalPivot = slot.transform.Find("RotationPivot/AnimalPivot");

        GameObject meshObject = (GameObject)Instantiate(ic.goUnlocked.meshPrefab);
        ic.meshObject = meshObject;
		meshObject.transform.parent = animalPivot;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.transform.localRotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one*scale;

        meshObject.GetComponent<Animator>().runtimeAnimatorController = ic.animatorController;
        nameAudio = ic.spokenName;

        /*if (ic.gender == "Female")
        {
            ic.genderTextureMale.SetActive(false);
            ic.genderTextureFemale.SetActive(true);
        }
        else if (ic.gender == "Male")
        {
            ic.genderTextureMale.SetActive(true);
            ic.genderTextureFemale.SetActive(false);
        }
        else
        {
        */
            ic.genderTextureFemale.SetActive(false);
            ic.genderTextureMale.SetActive(false);
        //}

        Renderer[] renderers = meshObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.sharedMaterial = ic.goUnlocked.material;
        }

        return meshObject;
    }

    private GameObject FillFemale(GameObject slot, InfoCards ic, float scale = 1)
    {
        //slot.name = ic.goUnlocked.ToString();

        Transform animalPivot = slot.transform.Find("RotationPivot/AnimalPivot");

        GameObject meshObject = (GameObject)Instantiate(ic.goUnlocked.meshPrefab);
        ic.meshObject = meshObject;
        meshObject.transform.parent = animalPivot;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.transform.localRotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one*scale;

        meshObject.GetComponent<Animator>().runtimeAnimatorController = ic.goUnlocked.femaleEquivalent.GetComponent<CardDef>().animatorController;

        ic.genderTextureFemale.SetActive(false);
        ic.genderTextureMale.SetActive(false);

        Renderer[] renderers = meshObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.sharedMaterial = ic.goUnlocked.femaleEquivalent.GetComponent<CardDef>().material;
        }

        return meshObject;
    }

    private IEnumerator CardFlip(Animator animator, bool isFlipped)
    {
        yield return new WaitForSeconds(0.5f);
        Flip(animator, isFlipped);
        yield return new WaitForSeconds(0.4f);
        nameText.gameObject.SetActive(true);
    }

    private void Flip(Animator animator, bool isFlipped)
    {
        if (isFlipped)
        {
            animator.CrossFade("FlipToFront", 0.5f);
        }
        else
        {
            animator.CrossFade("FlipToBack", 0.5f);
        }
    }

    private void Flip(bool isFlipped)
    {
        foreach(GameObject slot in goSlots)
        {
            if(isFlipped)
            {
                slot.GetComponent<Animator>().CrossFade("FlipToFront", 0.5f);
            }
            else
            {
                slot.GetComponent<Animator>().CrossFade("FlipToBack", 0.5f);
            }
        }
    }

    private void ResetState()
    {
		backButton.gameObject.SetActive(true);
		factSheetBackButton.gameObject.SetActive(false);

		foreach (GameObject slot in goSlots)
        {
            currentInfoBackground = 0;
            currentPlace = 0;
            isEnd = false;
            Animator animator = slot.GetComponent<Animator>();
            animator.Play("FlipToFront", 0, 0);
        }
    }
}
