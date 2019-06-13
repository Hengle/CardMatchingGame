using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KeenTween;
using System.Collections.ObjectModel;

[CreateAssetMenu(fileName = "RegionInfoManager", menuName = "RegionInfoManager")]
public class RegionInfoManager : ScriptableObject
{
	private static RegionInfoManager _instance;
	public static RegionInfoManager instance => _instance ? _instance : _instance = Resources.Load<RegionInfoManager>("RegionInfoManager");

	public List<RegionInfo> regionInfos = new List<RegionInfo>();
}
