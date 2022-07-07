using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mirror.PlanetaryCombat
{
	public class PlayerNameplate : MonoBehaviour
	{

		[SerializeField]
		private Text usernameText;

		[SerializeField]
		private RectTransform healthBarFill;

		[SerializeField]
		private Player player;

		// Update is called once per frame
		void Update()
		{
			usernameText.text = player.username;
			healthBarFill.localScale = new Vector3(player.GetHealthPct(), 1f, 1f);
		}

	}
}