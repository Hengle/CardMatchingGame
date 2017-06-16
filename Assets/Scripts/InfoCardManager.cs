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
    public GameObject goBackButton;

    public Text animalNameText;
    public Text animalDescriptionText;
    public RawImage animalMap;
    public Text animalPercentage;

    private int currentPlace = 0;
    private bool isFlipping = false;
    private bool isEnd = false;

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
                    if (isEnd)
                    {
                        currentPlace = 0;
                        isEnd = false;
                    }
                    Arrow("Right");
                }
            }
        }

        if(!goBackButton.GetComponent<Info>().factSheet)
        {
            foreach (GameObject slot in goSlots)
            {
                slot.SetActive(true);
            }
            goBackground.SetActive(false);
        }
    }

    public void ShowFactSheet(InfoCards animal)
    {
        goBackButton.GetComponent<Info>().factSheet = true;
        foreach(GameObject slot in goSlots)
        {
            slot.SetActive(false);
        }
        goBackground.SetActive(true);

        var animalSlot = goBackground.transform.Find("animal");

        if (animalSlot.Find("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalSlot.Find("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);

        FillFactSheet(animal.goUnlocked.name);
        Fill(animalSlot.gameObject, animal);
        animalSlot.GetComponent<Animator>().Play("FlipToFront");
        //Flip(animalSlot.GetComponent<Animator>(), true);
    }

    private void FillFactSheet(string animalName)
    {
        // Name to display
        string name = (from animal in InfoText.getDescriptions() where animal.name == animalName select animal.displayName).First();

        if (name == "")
            name = (from animal in InfoText.getDescriptions() where animal.name == animalName select animal.name).First();

        // Description to display
        string description = (from animal in InfoText.getDescriptions() where animal.name == animalName select animal.description).First();

        // String of map name
        string image = (from animal in InfoText.getDescriptions() where animal.name == animalName select animal.map).First();

        // Max number of each animal in game
        float maxCount = (from animal in InfoText.getDescriptions() where animal.name == animalName select animal.maxCount).First();

        // Sets name and description
        animalNameText.text = name;
        animalDescriptionText.text = description;

        // Creates path to texture
        char seperator = Path.DirectorySeparatorChar;
        string path = "Map" + seperator + "InfoMaps" + seperator +"Textures" + seperator + image;
        animalMap.texture = (Texture)Resources.Load(path);

        //@TODO need to hook up the unlock counter 
        int counter = 1;
        animalPercentage.text = "Unlocked: 1/" + maxCount + " Percentage: " + (counter / maxCount) * 100 + "%";
    }

    private void Arrow(string direction)
    {
        Debug.Log(direction);
        isFlipping = true;
        Flip(false);
        FillSlots();
    }

    private  void FillSlots()
    {
        foreach (GameObject slot in goSlots)
        {
            InfoCards ic = slot.GetComponent<InfoCards>();

            if (ic.meshObject != null)
                GameObject.Destroy(ic.meshObject);

            if (currentPlace >= goUnlocked.Length)
            {
                isEnd = true;
                continue;
            }

            if (currentPlace < 0)
                currentPlace = goUnlocked.Length - 8;

            ic.goUnlocked = goUnlocked[currentPlace];

            Fill(slot, ic);

            if (!PlayerPrefs.HasKey(ic.goUnlocked.ToString()))
            {
                StartCoroutine(CardFlip(slot.GetComponent<Animator>(), true));
                }

            currentPlace++;
        }
        isFlipping = false;
    }

    private void Fill(GameObject slot, InfoCards ic)
    {
        //slot.name = ic.goUnlocked.ToString();

        GameObject meshObject = (GameObject)Instantiate(ic.goUnlocked.meshPrefab);
        ic.meshObject = meshObject;
        meshObject.transform.parent = slot.transform.Find("RotationPivot/AnimalPivot");
        meshObject.transform.localPosition = Vector3.zero;
        meshObject.transform.localRotation = Quaternion.identity;
        meshObject.transform.localScale = Vector3.one;

        Renderer[] renderers = meshObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.sharedMaterial = ic.goUnlocked.material;
        }
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
            animator.Play("New State", 0, 0);
        }
    }
}
