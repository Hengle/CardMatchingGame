using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCardManager : MonoBehaviour
{
    public GameObject goLeftArrow;
    public GameObject goRightArrow;

    public CardDef[] goUnlocked;
    public GameObject[] goSlots;

    private int currentPlace = 0;
    private bool isFlipping = false;

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
                    currentPlace -= goUnlocked.Length;

                    if (currentPlace < 0)
                        currentPlace =- 16;

                    Arrow("Left");
                }
                else if(hit.collider.gameObject.name == goRightArrow.name)
                {
                    Arrow("Right");
                }
            }
        }
    }

    private void Arrow(string direction)
    {
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

            ic.goUnlocked = goUnlocked[currentPlace];

            slot.name = currentPlace.ToString();

            currentPlace++;

            if (currentPlace >= goUnlocked.Length)
                currentPlace = 0;

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

            if (!PlayerPrefs.HasKey(ic.goUnlocked.ToString()))
            {
                StartCoroutine(CardFlip(slot.GetComponent<Animator>(), true));
            }
        }
        isFlipping = false;
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
            Animator animator = slot.GetComponent<Animator>();
            animator.Play("New State", 0, 0);
        }
    }
}
