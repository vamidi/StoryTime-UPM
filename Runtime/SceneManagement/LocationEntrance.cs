﻿using UnityEngine;

namespace DatabaseSync
{

	public class LocationEntrance : MonoBehaviour
	{
		[Header("Asset References")]
		[SerializeField] private PathSO entrancePath;

		public PathSO EntrancePath => entrancePath;
	}
}
