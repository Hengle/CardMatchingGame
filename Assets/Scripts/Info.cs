using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    public Transform infoPage;
    public Transform startPoint;
    public bool factSheet = false;
    public AudioClip clip;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Info region = TryClickRegion(Input.mousePosition);
            if (region)
            {
                OnClickRegion(region);
            }
        }
    }

    Info TryClickRegion(Vector2 screenPoint)
    {
        Info region = null;

        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            region = hit.collider.gameObject.GetComponentInParent<Info>();
        }

        return region;
    }

    public void OnClickRegion(Info region)
    {
        LoadInfo(infoPage, startPoint);
    }

    private void LoadInfo(Transform infoPage, Transform startPoint)
    {
        OneShotAudio.Play(clip, 0, GameSettings.Audio.sfxVolume);
        if (!factSheet)
        {
            infoPage.gameObject.SetActive(true);
            startPoint.gameObject.SetActive(false);
        }
        else
        {
            factSheet = false;
        }
    }
}
