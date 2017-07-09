using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;

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
		animator.CrossFade("Match", 0.5f);
		
		StartCoroutine(AnimateAway());
	}

	public void MoveTo(Vector3 position, Vector3 pivot, float time)
	{
		Vector3 originalPosition = transform.position;
		Tween tween = new Tween(null, 0, 1, time, new CurveSinusoidal(TweenCurveMode.InOut), t =>
		{
			if (!this)
			{
				return;
			}
			Quaternion rot = Quaternion.AngleAxis(t.currentValue*180, Vector3.forward);

			Vector3 toOriginalPosition = originalPosition-pivot;
			
			transform.position = pivot+rot*toOriginalPosition;
			transform.position -= Vector3.forward*Mathf.PingPong(t.currentValue, 0.5f)*5;
		});
		tween.onFinish += t =>
		{
			transform.position = position;
		};
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
