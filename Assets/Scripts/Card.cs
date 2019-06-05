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
	public Transform adjacentLionPivot;
	public SpriteRenderer backSpriteRenderer;

	[System.NonSerialized]
	public bool _isMatched;
	public bool isMatched { get; private set; }

	[System.NonSerialized]
	public int exposedTurn = -1;
	public bool hasBeenExposed => exposedTurn >= 0;

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

        OneShotAudio.Play(clipRun, 0, GameSettings.Audio.sfxVolume);

		Tween runTween = new Tween(null, 0, 1, 3, null, t =>
		{
			if (!animalPivot)
			{
				return;
			}
			animalPivot.transform.localPosition = new Vector3(Mathf.Lerp(0, 10, t.currentValue), 0, Mathf.Lerp(0, 0.225f, t.currentValue*5));
		});
		runTween.onFinish += t =>
		{
			if (animalPivot)
			{
				Destroy(animalPivot.gameObject);
			}
		};

		if (adjacentLionPivot)
		{
			yield return new WaitForSeconds(0.25f);

			if (Game.current.GetAdjacentCards(tilePositionX, tilePositionY).Any(v => v.cardDef is LionCardDef))
			{
				adjacentLionPivot.localScale = Vector3.zero;
				adjacentLionPivot.gameObject.SetActive(true);
				Tween tween = new Tween(null, 0, 1, 0.25f, new CurveCubic(TweenCurveMode.Out), t =>
				{
					if (!adjacentLionPivot)
					{
						return;
					}
					adjacentLionPivot.localScale = Vector3.one*t.currentValue;
				});
			}
		}
	}

	public void AnimateDance() {
		animalAnimator.SetTrigger("Dance");
	}

	public void Expose(int turn)
	{
		if (!hasBeenExposed)
		{
			exposedTurn = turn;
			StartCoroutine(OnExposedAsync(0.5f));
		}
	}

	private IEnumerator OnExposedAsync(float delay)
	{
		yield return new WaitForSeconds(delay);
		cardDef.OnExposed(this);
	}
}
