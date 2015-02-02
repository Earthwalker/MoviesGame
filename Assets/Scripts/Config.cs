using UnityEngine;
using System.Collections.Generic;

namespace Movies
{
	/// <summary>
	/// Holds the current game configuration
	/// 
	/// Attached to Controller
	/// </summary>
	public class Config : MonoBehaviour
	{
		[Header("Items Per Game")]

		/// <summary>
		/// Number of customers in each game
		/// </summary>
		public int CustomerNumber;

		/// <summary>
		/// Number movies in each game
		/// </summary>
		public int MovieNumber;


		[Header("Attributes Per Item")]

		/// <summary>
		/// Number of attributes a customer can have
		/// </summary>
		public int CustomerAttributeNumber;

		/// <summary>
		/// Number of attributes a movie can have
		/// </summary>
		public int MovieAttributeNumber;


		[Header("Attribute Totals")]

		/// <summary>
		/// Total number of genres
		/// </summary>
		public int GenreTotal;

		/// <summary>
		/// Total number of actors
		/// </summary>
		public int ActorTotal;

		[Header("Time")]

		/// <summary>
		/// Questions allowed per game
		/// </summary>
		public int timeAllowed;

		[Header("Knowledge")]

		/// <summary>
		/// The amount each customer knows compared to the number he has (higher value is easier)
		/// </summary>
		public int KnowledgeMultiplier;
    }
}