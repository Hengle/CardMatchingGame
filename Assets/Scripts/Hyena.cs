using UnityEngine;
using System.Collections;

public class Hyena:MonoBehaviour
{
	public Animator animator;
	public Animator sausageAnimator;
    public AudioClip clipLaugh;
    public AudioClip clipFall;
	private AudioSource audioSource;

	void Awake()
	{
		audioSource = gameObject.GetComponent<AudioSource>();
	}

	public void Laugh()
	{
		animator.SetTrigger("Laugh");
        audioSource.clip = clipLaugh;
		audioSource.Play();
    }
	
	public void SausageFall()
	{
		StartCoroutine(SausageFallAsync());
	}

	private IEnumerator SausageFallAsync()
	{
		sausageAnimator.SetTrigger("Fall");
		yield return new WaitForSeconds(0.5f);
        audioSource.clip = clipFall;
        audioSource.Play();
        animator.SetTrigger("Fall");
	}
}
