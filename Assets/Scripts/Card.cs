using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card:MonoBehaviour {
	public CardDef cardDef;
	public int tilePositionX = -1;
	public int tilePositionY = -1;

	public bool _isMatched;
	public bool isMatched {
		get {
			return _isMatched;
		}
		set {
			_isMatched = value;

			if (_isMatched) {
				AnimateDance();
			}
		}
	}

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

	//public void AnimateIdle() {
	//	DisableAnimatorStates();
	//	animator.SetBool("Idle", true);
	//}

	public void AnimateDance() {
		animator.SetTrigger("Dance");
		//DisableAnimatorStates();
		//animator.SetBool("Idle", true);
	}

	//private void DisableAnimatorStates() {
	//	animator.SetBool("Idle", false);
	//	animator.SetBool("Dance", false);
	//}

	void Awake() {
		isFlipped = false;
	}
}
