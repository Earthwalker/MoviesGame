using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	public class Matching : MonoBehaviour
	{
		#region inspector

		public GameObject finishedButton;

		#endregion

		/// <summary>
		/// Holds the matches the player makes during the game
		/// </summary>
		public Dictionary<Customer, Movie> matches;

		/// <summary>
		/// Whether we are ready to start a new game
		/// </summary>
		public bool readyForNewGame = false;

		/// <summary>
		/// Resets our stored matches
		/// </summary>
		public void Reset()
		{
			// create our matches dictionary
			matches = new Dictionary<Customer, Movie>();

			// make sure the player cannot finish the game yet
			finishedButton.SetActive(false);
        }

		/// <summary>
		/// Match a movie to the current customer
		/// </summary>
		/// <param name="movie"></param>
		public void Match(Movie movie)
		{
			// get the current recipient in chat
			GameObject customer = GetComponent<Chat>().CurrentRecipient;

			// check to make sure we HAVE a customer to match the movie with
			if (customer == null)
				return;

			// remove any existing matches involving this movie
			if (matches.ContainsValue(movie))
			{
				foreach (var match in matches)
				{
					// only remove matches involving other customers
					if (match.Value == movie && match.Key != customer.GetComponent<Customer>())
					{
						matches.Remove(match.Key);
						break;
					}
				}
			}

			// if the customer already has a match, change it instead of adding a new one
			if (matches.ContainsKey(customer.GetComponent<Customer>()))
                matches[customer.GetComponent<Customer>()] = movie;
			else
				matches.Add(customer.GetComponent<Customer>(), movie);

			// allow the user to press the magical finished button!
			finishedButton.SetActive(CheckReady());
        }

		/// <summary>
		/// Update the visual match info for the current customer
		/// </summary>
		/// <param name="customer"></param>
		public void ShowCustomerMatch(Customer customer)
		{
			// toggle our match if we have one
			Movie movie;
			if (matches.TryGetValue(customer, out movie))
				movie.GetComponent<Toggle>().isOn = true;
			else
				GameObject.Find("EmptyToggle").GetComponent<Toggle>().isOn = true;
		}

		/// <summary>
		/// Checks if the player has matched a movie with every customer
		/// </summary>
		/// <returns>Whether we can finish the game</returns>
		bool CheckReady()
		{
			// bypass the check if we have already finished the game
			if (readyForNewGame)
				return true;

			// loop through each customer and make sure he has a movie match
			foreach (var customer in GameObject.FindGameObjectsWithTag("Customer"))
			{
				if (!matches.ContainsKey(customer.GetComponent<Customer>()))
					return false;
			}

			return true;
		}

		/// <summary>
		/// Triggered when the finished button is pressed and we have a movie to every customer
		/// </summary>
		public void Finished()
		{
			// if we've already finished the game and viewed the scores, restart the game
			if (readyForNewGame)
				Application.LoadLevel(Application.loadedLevel);

			// show details about the customers
			foreach (var customer in GameObject.FindGameObjectsWithTag("Customer"))
				customer.transform.FindChild("Tidbits").gameObject.SetActive(true);

			// calculate the player's score
			GetComponent<Scoring>().CalculatePlayerScore(matches);

			// calculate the highest possible score
			GetComponent<Scoring>().CalculateHighestScore();

			readyForNewGame = true;
		}
	}
}
