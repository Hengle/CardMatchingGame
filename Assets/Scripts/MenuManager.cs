using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour
{
	public Transform levelSelectTransform;
	public Transform splashScreenTransform;
    public Transform infoTransform;

	private void Update()
	{
		if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
		{
			if (splashScreenTransform.gameObject.activeSelf)
			{
				splashScreenTransform.gameObject.SetActive(false);
				levelSelectTransform.gameObject.SetActive(true);
			}
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (levelSelectTransform.gameObject.activeSelf && !infoTransform.gameObject.activeSelf)
			{
				levelSelectTransform.gameObject.SetActive(false);
				splashScreenTransform.gameObject.SetActive(true);
			}

            if (infoTransform.gameObject.activeSelf)
            {
                infoTransform.gameObject.SetActive(false);
                levelSelectTransform.gameObject.SetActive(true);
            }
        }
    }
}
