using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card:MonoBehaviour {
	public CardDef cardDef;
	public Transform animalPivot;
	public int tilePositionX = -1;
	public int tilePositionY = -1;

	public bool _isMatched;
	public bool isMatched { get; private set; }

	private bool _isFlipped;
	public bool isFlipped {
		get {
			return _isFlipped;
		}
		set {
			_isFlipped = value;
			
			if (_isFlipped) {
				animator.CrossFade("FlipToFront", 0.5f);
			}
			else {
				animator.CrossFade("FlipToBack", 0.5f);
			}
		}
	}

	private Animator _animator;
	public Animator animator
	{
		get
		{
			return _animator ? _animator : _animator = gameObject.GetComponent<Animator>();
		}
	}

	private Animator _animalAnimator;
	public Animator animalAnimator {
		get {
			if (!_animalAnimator) {
				_animalAnimator = transform.Find("RotationPivot/AnimalPivot").GetChild(0).GetComponent<Animator>();
			}
			return _animalAnimator;
		}
	}

	private Animation _scaleAnimation;
	public Animation scaleAnimation {
		get {
			if (!_scaleAnimation) {
				_scaleAnimation = gameObject.GetComponent<Animation>();
			}
			return _scaleAnimation;
		}
	}

	public void OnMatch(Card other)
	{
		isMatched = true;
		AnimateDance();
		//LeanTween.scale(gameObject, Vector3.one*1.25f, 0.25f).setLoopPingPong().setLoopCount(2).setEase(LeanTweenType.easeOutCubic);
		animator.CrossFade("Match", 0.5f);
		
		StartCoroutine(AnimateAway());
	}

	public void MoveTo(Vector3 position, float time)
	{
		StartCoroutine(MoveToAsync(position, time));
	}

	private IEnumerator MoveToAsync(Vector3 position, float time)
	{
		Vector3 originalPosition = transform.position;
		float counter = 0;
		while (counter < time)
		{
			counter += Time.deltaTime;
			float ratio = counter/time;
			transform.position = Vector3.Lerp(originalPosition, position, Mathf.SmoothStep(0, 1, ratio));
			yield return 0;
		}
	}

	private IEnumerator AnimateAway()
	{
		yield return new WaitForSeconds(2);
		float counter = 0;
		while (counter < 3)
		{
			counter += Time.deltaTime;
			float ratio = counter/3;
			animalPivot.transform.localPosition = new Vector3(Mathf.Lerp(0, 10, ratio), 0, Mathf.Lerp(0, 0.225f, ratio*5));
			yield return 0;
		}

		Destroy(animalPivot.gameObject);
	}

	public void AnimateDance() {
		animalAnimator.SetTrigger("Dance");
	}
	

	void Awake() {
		isFlipped = false;
	}
}
