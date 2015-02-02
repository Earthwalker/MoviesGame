using UnityEngine;

namespace Movies
{
	/// <summary>
	/// Static class to handle menu events
	/// 
	/// Accessed by UI
	/// </summary>
	public class UIHelper : MonoBehaviour
	{
		/// <summary>
		/// Exits the game
		/// </summary>
		public void Exit()
		{
			Application.Quit();
		}
	}
}
