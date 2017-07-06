namespace KeenTween {
	public class CurveBounce : TweenCurve
	{
		public CurveBounce(TweenCurveMode easeMode) : base(easeMode)
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

		public static float EaseOut( float t, float b, float c, float d )
		{
			if( ( t /= d ) < ( 1 / 2.75 ) )
			{
				return c * ( 7.5625f * t * t ) + b;
			}
			else if( t < ( 2 / 2.75 ) )
			{
				return c * ( 7.5625f * ( t -= ( 1.5f / 2.75f ) ) * t + .75f ) + b;
			}
			else if( t < ( 2.5 / 2.75 ) )
			{
				return c * ( 7.5625f * ( t -= ( 2.25f / 2.75f ) ) * t + .9375f ) + b;
			}
			else
			{
				return c * ( 7.5625f * ( t -= ( 2.625f / 2.75f ) ) * t + .984375f ) + b;
			}
		}

		public static float EaseIn( float t, float b, float c, float d )
		{
			return c - EaseOut( d - t, 0, c, d ) + b;
		}

		public static float EaseInOut( float t, float b, float c, float d )
		{
			if( t < d / 2 )
				return EaseIn( t * 2, 0, c, d ) * 0.5f + b;
			else
				return EaseOut( t * 2 - d, 0, c, d ) * .5f + c * 0.5f + b;
		}
	}
}