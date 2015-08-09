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
				LeanTween.rotate(gameObject, new Vector3(0, 180, 0), 0.25f);
			}
			else {
				LeanTween.rotate(gameObject, new Vector3(0, 0, 0), 0.25f);
			}
		}
	}

	private Animator _animator;
	public Animator animator {
		get {
			if (!_animator) {
				_animator = transform.Find("AnimalPivot").GetChild(0).GetComponent<Animator>();
			}
			return _animator;
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
		LeanTween.scale(gameObject, Vector3.one*1.25f, 0.25f).setLoopPingPong().setLoopCount(2).setEase(LeanTweenType.easeOutCubic);

		StartCoroutine(AnimateAway());
	}

	private IEnumerator AnimateAway()
	{
		yield return new WaitForSeconds(2);
		LeanTween.moveLocalX(animalPivot.gameObject, animalPivot.localPosition.x+10, 3);
		LeanTween.moveLocalZ(animalPivot.gameObject, animalPivot.localPosition.z+0.25f, 0.25f);
		yield return new WaitForSeconds(3);
		Destroy(animalPivot.gameObject);
	}

	public void AnimateDance() {
		animator.SetTrigger("Dance");
	}
	

	void Awake() {
		isFlipped = false;
	}
}
