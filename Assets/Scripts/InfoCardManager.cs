﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCardManager : MonoBehaviour
{
    public GameObject goLeftArrow;
    public GameObject goRightArrow;

    public CardDef[] goUnlocked;
    public GameObject[] goSlots;
    public GameObject goBackground;
    public GameObject goBackButton;

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
        var animalSlot = goBackground.transform.FindChild("animal");

        if (animalSlot.FindChild("RotationPivot/AnimalPivot").childCount > 0)
            Destroy(animalSlot.FindChild("RotationPivot/AnimalPivot").GetComponentInChildren<Animator>().gameObject);

        Fill(animalSlot.gameObject, animal);
        animalSlot.GetComponent<Animator>().Play("FlipToFront");
        //Flip(animalSlot.GetComponent<Animator>(), true);
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