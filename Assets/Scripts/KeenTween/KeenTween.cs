using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Reflection;
using System;
using System.Collections.ObjectModel;

namespace KeenTween
{
	public enum TweenCurveMode { In, Out, InOut }
	public abstract class TweenCurve
	{
		public TweenCurve(TweenCurveMode easeMode)
		{
			this.easeMode = easeMode;
		}

		public TweenCurveMode easeMode { get; set; }
		public abstract float SampleIn(float position);
		public abstract float SampleOut(float position);
		public abstract float SampleInOut(float position);

		public float Sample(float position)
		{
			if (easeMode == TweenCurveMode.In)
			{
				return SampleIn(position);
			}
			else if (easeMode == TweenCurveMode.Out)
			{
				return SampleOut(position);
			}
			else if (easeMode == TweenCurveMode.InOut)
			{
				return SampleInOut(position);
			}

			throw new Exception("Unknown EaseMode \""+easeMode+"\"");
		}

		private static Dictionary<string, Type> typeLookupDict;
		private static void ValidateTypeLookupDict()
		{
			if (typeLookupDict == null)
			{
				typeLookupDict = new Dictionary<string, Type>();
				foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					//Accessing some assemblies like this can sometimes throw exceptions...
					try
					{
						foreach (var type in assembly.GetTypes())
						{
							try
							{
								if (typeof(TweenCurve).IsAssignableFrom(type))
								{
									string name = type.Name;
									if (!name.StartsWith("Ease"))
									{
										continue;
									}
									name = name.Substring("Ease".Length);

									typeLookupDict.Add(name, type);
								}
							}
							catch (Exception)
							{

							}
						}
					}
					catch (Exception)
					{

					}
				}
			}
		}

		public static Type Find(string name)
		{
			ValidateTypeLookupDict();

			Type type;
			typeLookupDict.TryGetValue(name, out type);
			return type;
		}

		public static TweenCurve FromName(string name, TweenCurveMode curveMode)
		{
			Type type = Find(name);
			if (type == null)
			{
				return null;
			}

			return (TweenCurve)Activator.CreateInstance(type, curveMode);
		}
	}
	
	public class Tween
	{
		static private List<Tween> tweens = new List<Tween>();
		static private CurveLinear defaultCurve = new CurveLinear(TweenCurveMode.InOut);
		static Tween()
		{
			TweenTicker.ValidateInstance();
		}
		
		public enum TweenUpdateMode { Update, FixedUpdate };
		public enum TweenLoopMode { None, Loop, PingPong }

		public delegate void OnTweenStartCallback(Tween tween);
		public delegate void OnTweenTickDelegate(Tween tween);
		public delegate void OnTweenFinishCallback(Tween tween);

		public TweenLoopMode loopMode { get; set; }
		public float from { get; set; }
		public float to { get; set; }
		public float length { get; set; }
		public TweenCurve curve { get; set; }

		public List<Tween> childList = new List<Tween>();
		public ReadOnlyCollection<Tween> children { get; private set; }

		//public event OnTweenFinishCallback onStart;
		public event OnTweenTickDelegate onTick;
		public event OnTweenFinishCallback onFinish;

		public Tween(Tween parent, float from, float to, float length, TweenCurve curve = null, OnTweenTickDelegate onTick = null)
		{
			children = new ReadOnlyCollection<Tween>(childList);

			this.from = from;
			this.to = to;
			this.length = length;
			this.curve = curve;

			if (onTick != null)
			{
				this.onTick += onTick;
			}

			if (parent == null)
			{
				tweens.Add(this);
			}
			else
			{
				parent.childList.Add(this);
			}
		}

		private bool OnTick()
		{
			try
			{
				if (onTick != null)
				{
					onTick(this);
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return false;
			}
			return true;
		}

		private bool OnFinish()
		{
			try
			{
				if (onFinish != null)
				{
					onFinish(this);
				}
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				return false;
			}
			return true;
		}

		public float currentTime { get; set; }
		public float currentPosition
		{
			get
			{
				return Mathf.Clamp01(currentTime/length);
			}
		}

		public float currentValue
		{
			get
			{
				TweenCurve c = curve != null ? curve : defaultCurve;
				return Mathf.LerpUnclamped(from, to, c.Sample(currentPosition));
			}
		}

		public bool isDone { get; private set; }
		
		private void TickInstance(float deltaTime)
		{
			if (isDone)
			{
				return;
			}

			currentTime += deltaTime;

			if (!OnTick())
			{
				Cancel();
			}

			if (currentTime >= length)
			{
				Finish();
			}
		}

		public void Cancel()
		{
			currentTime = 1;
			isDone = true;
		}

		public void Finish()
		{
			OnFinish();
			Cancel();

			foreach (var child in childList)
			{
				tweens.Add(child);
			}
			childList.Clear();
		}

		public static void Tick(float deltaTime)
		{
			for (int i = 0; i < tweens.Count; i++)
			{
				Tween tween = tweens[i];
				tween.TickInstance(deltaTime);

				if (tween.isDone)
				{
					tweens.RemoveAt(i);
					i--;
				}
			}
		}

		public static void Clear()
		{
			tweens.Clear();
		}
	}

	public class TweenTicker : MonoBehaviour
	{
		private static TweenTicker instance;
		public static void ValidateInstance()
		{
			if (!instance)
			{
				instance = FindObjectOfType<TweenTicker>();

				if (!instance)
				{
					instance = new GameObject("TweenTicker").AddComponent<TweenTicker>();
					DontDestroyOnLoad(instance);
				}
			}
		}

		private void Update()
		{
			Tween.Tick(Time.deltaTime);
		}
	}
}