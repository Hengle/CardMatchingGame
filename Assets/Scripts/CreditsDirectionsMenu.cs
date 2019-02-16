using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsDirectionsMenu : MonoBehaviour {

    public Sprite[] images;
    public Button left;
    public Button right;

    private int index = 0;
    private Sprite mainImage;

    // Use this for initialization
    void Start()
    {
        index = 0;
        mainImage = images[index];
    }

    // Update is called once per frame
    void Update()
    {
        int index = System.Array.IndexOf(images, mainImage);
        gameObject.GetComponent<Image>().sprite = mainImage;
    }

    public void RightButton()
    {
        index++;
        if (index > images.Length - 1)
            index = 0;

        mainImage = images[index];
    }

    public void LeftButton()
    {
        index--;
        if (index < 0)
            index = images.Length - 1;

        mainImage = images[index];
    }
}
