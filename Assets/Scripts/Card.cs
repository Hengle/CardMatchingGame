using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Card:MonoBehaviour {
	public Card prefab;
	public int tilePositionX = -1;
	public int tilePositionY = -1;

	public bool isMatched {get; set;}

	private bool _isFlipped;
	public bool isFlipped {
		get {
			return _isFlipped;
		}
		set {
			//if (value == _isFlipped) {
			//	return;
			//}
			_isFlipped = value;

			if (_isFlipped) {
				//transform.eulerAngles = new Vector3(0, 0, 0);
				LeanTween.rotate(gameObject, new Vector3(0, 0, 0), 0.25f);
			}
			else {
				//transform.eulerAngles = new Vector3(0, 180, 0);
				LeanTween.rotate(gameObject, new Vector3(0, 180, 0), 0.25f);
			}
		}
	}

	void Awake() {
		isFlipped = false;
	}
}
