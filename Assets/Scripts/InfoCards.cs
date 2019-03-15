using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCards : MonoBehaviour
{
    public CardDef goUnlocked;
    public Transform animalAnimator;
    public RuntimeAnimatorController animatorController;
    public GameObject meshObject;
    public AudioClip spokenName;
    public string displayName;
    public string gender;
    public GameObject genderTextureMale;
    public GameObject genderTextureFemale;
    public Texture2D[] infoTextures;

    public GameObject cards;
    public GameObject background;
    public Info backButton;
    public bool clickable;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InfoCards region = TryClickRegion(Input.mousePosition);
            if (region && clickable)
            {
                OnClickRegion(region);
            }
        }
    }

    InfoCards TryClickRegion(Vector2 screenPoint)
    {
        InfoCards region = null;

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            region = hit.collider.gameObject.GetComponentInParent<InfoCards>();
        }

        return region;
    }

    public void OnClickRegion(InfoCards region)
    {
        if(region.gameObject.name == "Slot2")
            gameObject.transform.parent.GetComponent<InfoCardManager>().ShowFactSheet(region);
    }
}
