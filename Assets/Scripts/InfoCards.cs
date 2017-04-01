using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCards : MonoBehaviour
{
    public CardDef goUnlocked;
    public Transform animalAnimator;
    public GameObject meshObject;

    public GameObject cards;
    public GameObject background;
    public Info backButton;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            InfoCards region = TryClickRegion(Input.mousePosition);
            if (region)
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
        backButton.GetComponent<Info>().factSheet = true;
        cards.SetActive(false);
        background.SetActive(true);
    }
}
