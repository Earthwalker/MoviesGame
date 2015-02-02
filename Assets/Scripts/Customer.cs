using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	/// <summary>
	/// Customer has likes and dislikes stored as tidbits. Also has some knowledge of other customers' tidbits.
	/// 
	/// Attached to Customer
	/// </summary>
	public class Customer : MonoBehaviour
	{
		/// <summary>
		/// Tidbits about this customer
		/// </summary>
		public List<Tidbit> tidbits;

		/// <summary>
		/// Known tidbits of other customers
		/// </summary>
		public List<Tidbit> knownTidbits;

		/// <summary>
		/// Which movie is currently matched with customer
		/// </summary>
		public Movie movieMatch;

		/// <summary>
		/// Generate tidbits about this customer
		/// </summary>
		/// <param name="number">Number of attributes for each customer</param>
		/// <param name="availableAttributes">Available attributes to choose from</param>
		public void GenerateTidbits(int number, Dictionary<string, string> availableAttributes)
		{
			// create our tidbit list
			tidbits = new List<Tidbit>();

			// create a list of random indices so we know which attributes to take from the available ones
			var randomIndices = new List<int>();
			int rand;

			// if we can't add as our target amount, add as many as we can
			for (int i = 0; i < Mathf.Min(number, availableAttributes.Count); i++)
			{
				rand = Random.Range(0, availableAttributes.Count - 1);

				// make sure we don't add duplicates
				if (!randomIndices.Contains(rand))
					randomIndices.Add(rand);
			}

			// counter to find which attributes to add
			int counter = 0;

			foreach (var attribute in availableAttributes)
			{
				// if this is an attribute we chose, add it
				if (randomIndices.Contains(counter))
					// add the new attribute and how we feel about it
					tidbits.Add(new Tidbit(attribute.Key, attribute.Value, (Preference)Random.Range(1, (int)Preference.number), name));

				// increment the count
				counter++;
			}

			// add our newly created tidbits to our tidbits text box
			Text textbox = transform.FindChild("Tidbits").GetComponent<Text>();
			textbox.text = "";

			foreach (var tidbit in tidbits)
				textbox.text += tidbit.AttributePreference + " " + tidbit.AttributeName + "\n";
		}

		/// <summary>
		/// Add some tidbit about the other customers
		/// </summary>
		/// <param name="number"></param>
		/// <param name="customers">List of the other customers</param>
		public void AddTidbits(int number, List<Customer> customers)
		{
			// initialize if we haven't yet
			if (knownTidbits == null)
				knownTidbits = new List<Tidbit>();

			// add some random tidbits about the others
			for (int i = 0; i < number; i++)
				knownTidbits.Add(customers[Random.Range(0, customers.Count - 1)].GetRandomTidbit());
		}

		/// <summary>
		/// Finds a tidbit containing data
		/// </summary>
		/// <param name="data"></param>
		/// <returns>Matching tidbit</returns>
		public Tidbit GetKnownTidbit(string data)
		{
			// create a list of all matching tidbits
			var matchingTidbits = knownTidbits.FindAll(tidbit => tidbit.AttributeName.ToLower() == data.ToLower() ||
													   tidbit.AttributeType.ToLower() == data.ToLower() ||
													   tidbit.AttributePreference.ToString().ToLower() == data.ToLower() ||
													   tidbit.CustomerName.ToLower() == data.ToLower());

			// if we can't find any matches, output an empty tidbit
			if (matchingTidbits.Count == 0)
				return Tidbit.Empty();

			// pick a random one to output
			return matchingTidbits[Random.Range(0, matchingTidbits.Count - 1)];
		}

		/// <summary>
		/// Finds a random tidbit of this customer
		/// </summary>
		/// <returns></returns>
		public Tidbit GetRandomTidbit()
		{
			return tidbits[Random.Range(0, tidbits.Count - 1)];
		}
	}
}
