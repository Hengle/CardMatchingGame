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

    public Text animalNameText;
    public RawImage animalGender;
    public Text animalConservationText;
    public RawImage animalMap;
    public Text animalPercentage;
    public AudioSource antilopeAS;

	public Texture2D genderTextureMale;
	public Texture2D genderTextureFemale;

    private int currentPlace = 0;
    private bool isFlipping = false;
    private bool isEnd = false;
    private GameObject newMaleMesh;
    private GameObject newFemaleMesh;
    private bool animationEnd = true;
    private AudioClip nameAudio;

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
        Reset();
        FillSlots();
    }

    private void OnDisable()
    {
        Reset();
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
                    currentPlace -= 16;
                    Arrow("Left");
                }
                else if(hit.collider.gameObject.name == goRightArrow.name)
                {
					if (currentPlace >= goUnlocked.Length) {
						currentPlace = 0;
					}
					Arrow("Right");
                    if (isEnd)
                    {
                        currentPlace = 0;
                        isEnd = false;
                    }
                }
            }
        }

        if(!goBackButton.GetComponent<Info>().factSheet)
        {
            if (animationEnd && newMaleMesh && newFemaleMesh)
            {
                animationEnd = false;
                newMaleMesh.GetComponent<Animator>().SetTrigger("Run");
                StartCoroutine(AnimateWalkToAndAway(newMaleMesh, true));

                newFemaleMesh.GetComponent<Animator>().SetTrigger("Run");
                StartCoroutine(AnimateWalkToAndAway(newFemaleMesh, true));
            }
        }
    }

    public void ShowFactSheet(InfoCards animal)
    {
        goBackButton.GetComponent<Info>().factSheet = true;
        foreach(GameObject slot in goSlots)
        {
            slot.SetActive(false);
        }

        goBackButton.SetActive(false);
        goMenuBackground.SetActive(false);
        goBackground.SetActive(true);
        goLeftArrow.SetActive(false);
        goRightArrow.SetActive(false);

        var animalMaleSlot = goBackground.transform.Find("animal Male");

        if (animalMaleSlot.Find("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalMaleSlot.Find("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);
		
        FillFactSheet(animal.goUnlocked);
		newMaleMesh = Fill(animalMaleSlot.gameObject, animal);

        animalMaleSlot.gameObject.SetActive(true);
        animalMaleSlot.GetComponent<Animator>().Play("FlipToFront");
        newMaleMesh.GetComponent<Animator>().SetTrigger("Run");
        StartCoroutine(AnimateWalkToAndAway(newMaleMesh, false));

        var animalFemaleSlot = goBackground.transform.Find("animal Female");

        if (animalFemaleSlot.Find("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalFemaleSlot.Find("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);

        FillFactSheet(animal.goUnlocked);
        newFemaleMesh = FillFemale(animalFemaleSlot.gameObject, animal);

        animalFemaleSlot.gameObject.SetActive(true);
        animalFemaleSlot.GetComponent<Animator>().Play("FlipToFront");
        newFemaleMesh.GetComponent<Animator>().SetTrigger("Run");
        StartCoroutine(AnimateWalkToAndAway(newFemaleMesh, false));

        //Flip(animalSlot.GetComponent<Animator>(), true);
    }

    private IEnumerator AnimateWalkToAndAway(GameObject animalPivot, bool away)
    {
        yield return new WaitForSeconds(0.1f);
        float counter = 0;

        if (away)
        {
            goBackButton.SetActive(false);
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
            goBackButton.SetActive(true);
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
            goBackButton.SetActive(true);
            goMenuBackground.SetActive(true);
            animalPivot.GetComponent<Animator>().SetTrigger("Idle");
        }
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
        animalMap.texture = cardDef.infoMapTexture;
    }

    private void Arrow(string direction)
    {
        isFlipping = true;
        Flip(false);
        StartCoroutine(WaitFillSlots(0.5f));
    }

    private IEnumerator WaitFillSlots(float sec)
    {
        yield return new WaitForSeconds(sec);
        FillSlots();
    }

    private  void FillSlots()
    {
        foreach (GameObject slot in goSlots)
        {
            InfoCards ic = slot.GetComponent<InfoCards>();

			Transform animalPivot = ic.gameObject.transform.Find("RotationPivot/AnimalPivot");
			while (animalPivot.childCount > 0) {
				DestroyImmediate (animalPivot.GetChild(0).gameObject);
			}

            //if (ic.meshObject != null)
            //    GameObject.Destroy(ic.meshObject);

            if (currentPlace >= goUnlocked.Length)
            {
                isEnd = true;
                continue;
            }

            if (currentPlace < 0)
                currentPlace = goUnlocked.Length - 8;

            ic.goUnlocked = goUnlocked[currentPlace];
            ic.animatorController = goUnlocked[currentPlace].animatorController;
            ic.spokenName = goUnlocked[currentPlace].spokenName;
            ic.gender = goUnlocked[currentPlace].gender.ToString();

            Fill(ic.gameObject, ic);

            if (!PlayerPrefs.HasKey(ic.goUnlocked.ToString()))
            {
                StartCoroutine(CardFlip(slot.GetComponent<Animator>(), true));
            }

            currentPlace++;
        }
        isFlipping = false;
    }

    private GameObject Fill(GameObject slot, InfoCards ic)
    {
        //slot.name = ic.goUnlocked.ToString();

		Transform animalPivot = slot.transform.Find("RotationPivot/AnimalPivot");

        GameObject meshObject = (GameObject)Instantiate(ic.goUnlocked.meshPrefab);
        ic.meshObject = meshObject;
		meshObject.transform.parent = animalPivot;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.transform.localRotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one;

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

    private GameObject FillFemale(GameObject slot, InfoCards ic)
    {
        //slot.name = ic.goUnlocked.ToString();

        Transform animalPivot = slot.transform.Find("RotationPivot/AnimalPivot");

        GameObject meshObject = (GameObject)Instantiate(ic.goUnlocked.meshPrefab);
        ic.meshObject = meshObject;
        meshObject.transform.parent = animalPivot;
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.transform.localRotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one;

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

    private void Reset()
    {
        foreach (GameObject slot in goSlots)
        {
            currentPlace = 0;
            isEnd = false;
            Animator animator = slot.GetComponent<Animator>();
            animator.Play("FlipToFront", 0, 0);
        }
    }
}
