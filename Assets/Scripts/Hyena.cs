using UnityEngine;
using System.Collections;

public class Hyena:MonoBehaviour
{
	private Animator animator;
	private AudioSource audioSource;

	void Awake()
	{
		animator = gameObject.GetComponentInChildren<Animator>();
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void Laugh()
	{
		animator.SetTrigger("Laugh");
		audioSource.Play();
    }
}
