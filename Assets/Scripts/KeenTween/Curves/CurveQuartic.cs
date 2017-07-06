namespace KeenTween {
	public class CurveQuartic : TweenCurve
	{
		public CurveQuartic(TweenCurveMode easeMode) : base(easeMode)
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
			return c * ( t /= d ) * t * t * t + b;
		}

		public static float EaseOut( float t, float b, float c, float d )
		{
			return -c * ( ( t = t / d - 1 ) * t * t * t - 1 ) + b;
		}

		public static float EaseInOut( float t, float b, float c, float d )
		{
			if( ( t /= d / 2 ) < 1 )
			{
				return c / 2 * t * t * t * t + b;
			}
			return -c / 2 * ( ( t -= 2 ) * t * t * t - 2 ) + b;
		}
	}
}