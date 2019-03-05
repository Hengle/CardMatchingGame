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
    public AudioClip clipRear;
    public AudioClip clipRun;
	public Sprite exposedLionCardSprite;
	public SpriteRenderer cardBackSpriteRenderer;

	public bool _isMatched;
	public bool isMatched { get; private set; }

	public bool hasBeenExposed { get; private set; }

	private bool _isFlipped;
	public bool isFlipped {
		get {
			return _isFlipped;
		}
		set {
			if (value == _isFlipped)
			{
				return;
			}

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

	void Awake()
	{
		isFlipped = false;
	}

	public void OnMatch(Card other)
	{
		isMatched = true;
        AnimateDance();
        OneShotAudio.Play(clipRear, 0, GameSettings.Audio.sfxVolume);

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
			if (!transform)
			{
				return;
			}
			transform.position = position;
		};
	}

	private IEnumerator AnimateAway()
	{
		yield return new WaitForSeconds(2);
        float counter = 0;
        OneShotAudio.Play(clipRun, 0, GameSettings.Audio.sfxVolume);
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

	public void Expose()
	{
		if (!hasBeenExposed)
		{
			hasBeenExposed = true;
			StartCoroutine(OnExposedAsync(0.5f));
		}
	}

	private IEnumerator OnExposedAsync(float delay)
	{
		yield return new WaitForSeconds(delay);
		cardDef.OnExposed(this);
	}
}
