using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "RegionInfo", menuName = "RegionInfo")]
public class RegionInfo : ScriptableObject
{
	public string regionName = "";
	public List<Level> levels = new List<Level>();
	public List<Background> backgrounds = new List<Background>();

	public static RegionInfo GetLevelRegion(Level level)
	{
		foreach (var regionInfo in RegionInfoManager.instance.regionInfos)
		{
			if (regionInfo.levels.Contains(level))
			{
				return regionInfo;
			}
		}
		return null;
	}
}
