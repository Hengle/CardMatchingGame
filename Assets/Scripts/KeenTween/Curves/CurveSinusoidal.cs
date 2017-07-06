using System;
using UnityEngine;

namespace KeenTween {
	public class CurveSinusoidal : TweenCurve
	{
		public CurveSinusoidal(TweenCurveMode easeMode) : base(easeMode)
		{
		}

		public override float SampleIn(float position)
		{
			return EaseIn(position, 0, 1, 1);
		}

		public override float SampleOut(float position)
		{
			return EaseOut(position, 0, 1, 1);
		}

		public override float SampleInOut(float position)
		{
			return EaseInOut(position, 0, 1, 1);
		}

		public static float EaseIn( float t, float b, float c, float d )
		{
			return -c * (float)Math.Cos( t / d * ( Math.PI / 2 ) ) + c + b;
		}

		public static float EaseOut( float t, float b, float c, float d )
		{
			return c * (float)Math.Sin( t / d * ( Math.PI / 2 ) ) + b;
		}

		public static float EaseInOut( float t, float b, float c, float d )
		{
			return -c / 2 * ( (float)Math.Cos( Math.PI * t / d ) - 1 ) + b;
		}
	}
}