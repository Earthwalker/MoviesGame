using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Movies
{
	/// <summary>
	/// Handles the time of each game
	/// 
	/// Attached to TimeLeftText
	/// </summary>
	public class Clock : MonoBehaviour
	{
		/// <summary>
		/// Time remaining in the game
		/// </summary>
		public int TimeLeft;

		/// <summary>
		/// Adds a tick to the time
		/// </summary>
		/// <returns>Whether we are out of time</returns>
		public bool Tick()
		{
			TimeLeft--;

			if (TimeLeft <= 0)
			{
				GetComponent<Text>().text = "Finish up";
				return true;
			}

			GetComponent<Text>().text = "Questions left: " + TimeLeft.ToString();
			return false;
        }

		/// <summary>
		/// Resets the clock
		/// </summary>
		public void Reset()
		{
			// reset to our configured defaults
			TimeLeft = GameObject.FindGameObjectWithTag("GameController").GetComponent<Config>().timeAllowed;

			// set our text box
			GetComponent<Text>().text = "Enter a keyword";
		}
	}
}
