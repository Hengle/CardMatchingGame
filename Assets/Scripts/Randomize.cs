using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateBehaviours
{
	public class Randomize : StateMachineBehaviour
	{
		public string variableName = "";
		public int minValue = 0;
		public int maxValue = 1;

		override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.SetInteger(variableName, Random.Range(minValue, maxValue+1));
		}
	}
}
