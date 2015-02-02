using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	/// <summary>
	/// Handles scoring
	/// Attached to Controller
	/// </summary>
	public class Scoring : MonoBehaviour
	{
		#region inspector

		public GameObject scoreText;

		public GameObject maximumScoreText;

		#endregion

		/// <summary>
		/// Calculates the player's score
		/// </summary>
		/// <param name="matches"></param>
		public void CalculatePlayerScore(Dictionary<Customer, Movie> matches)
		{
			// show the player's score
			scoreText.GetComponent<Text>().text = "Score: " + CalculateScore(matches).ToString();
		}

		/// <summary>
		/// Calculate the maximum possible score
		/// </summary>
		public void CalculateHighestScore()
		{
			// highest score found so far
			int highestScore = 0;

			// get an array of all the customers
			var customers = GameObject.FindGameObjectsWithTag("Customer");

			// get the permutations of movies
			var moviePermutations = Util.GetPermutations<GameObject>(GameObject.FindGameObjectsWithTag("Movie")).ToList();

			// dictionary of matches to fill
			var matches = new Dictionary<Customer, Movie>();

			// loop through each permutation of movies
			for (int m = 0; m < moviePermutations.Count; m++)
			{
				// clear our matches to try another one
				matches.Clear();

				// loop through each customer and assign the designated movie
                for (int c = 0; c < customers.Length; c++)
					matches.Add(customers[c].GetComponent<Customer>(), moviePermutations[m].ElementAt(c).GetComponent<Movie>());

				// check if these matches give a higher score
				highestScore = Mathf.Max(highestScore, CalculateScore(matches));
			}

			// show the highest score possible
			maximumScoreText.GetComponent<Text>().text = "Highest Score: " + highestScore.ToString();
		}

		/// <summary>
		/// Calculate the score of the given matches
		/// </summary>
		/// <returns></returns>
		int CalculateScore(Dictionary<Customer, Movie> matches)
		{
			int score = 0;

			foreach (var match in matches)
			{
				foreach (var attribute in match.Value.GetComponent<Movie>().attributes)
					score += (int)match.Key.GetComponent<Customer>().tidbits.Find(tidbit => tidbit.AttributeName == attribute).AttributePreference;
			}

			return score;
		}
	}
}
