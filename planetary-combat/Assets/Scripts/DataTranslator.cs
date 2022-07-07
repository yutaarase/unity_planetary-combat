using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mirror.PlanetaryCombat
{
	public class DataTranslator : MonoBehaviour
	{

		private static string KILLS_ID = "[KILLS]";
		private static string DEATHS_ID = "[DEATHS]";

		public static string ValuesToData(int kills, int deaths)
		{
			return KILLS_ID + kills + "/" + DEATHS_ID + deaths;
		}

		public static int DataToKills(string data)
		{
			return int.Parse(DataToValue(data, KILLS_ID));
		}

		public static int DataToDeaths(string data)
		{
			return int.Parse(DataToValue(data, DEATHS_ID));
		}

		private static string DataToValue(string data, string id)
		{
			string[] pieces = data.Split('/');
			foreach (string piece in pieces)
			{
				if (piece.StartsWith(id))
				{
					return piece.Substring(id.Length);
				}
			}

			Debug.LogError(id + " not found in " + data);
			return "";
		}

	}
}