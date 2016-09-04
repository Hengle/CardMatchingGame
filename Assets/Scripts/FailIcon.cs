using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class FailIcon:MonoBehaviour
{
	public Image inactiveImage;
	public Image activeImage;

	public void SetActive(bool state)
	{
		if (state)
		{
			inactiveImage.gameObject.SetActive(false);
			activeImage.gameObject.SetActive(true);
		}
		else
		{
			inactiveImage.gameObject.SetActive(true);
			activeImage.gameObject.SetActive(false);
		}
	}
}
