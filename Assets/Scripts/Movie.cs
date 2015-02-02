using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	/// <summary>
	/// The movie object that will be matched with customers
	/// 
	/// Attached to Movie
	/// </summary>
	public class Movie : MonoBehaviour
	{
		/// <summary>
		/// Attributes of the movie
		/// </summary>
		public List<string> attributes;

		/// <summary>
		/// Generate attributes for this movie
		/// </summary>
		/// <param name="number">Number of attributes for each movie</param>
		/// <param name="availableAttributes">Available attributes to choose from</param>
		public void GenerateAttributes(int number, Dictionary<string, string> availableAttributes)
		{
			// create our attribute list
			attributes = new List<string>();

			// create a list of random indices so we know which attributes to take from the available ones
			var randomIndices = new List<int>();
			int rand;

			// loop through the available attributes to try and reach the target amount or the closest we can
			for (int i = 0; i < Mathf.Min(number, availableAttributes.Count); i++)
			{
				rand = Random.Range(0, availableAttributes.Count - 1);

				// make sure we don't add duplicates and we're just going to settle for less than the targeted amount
				// for extra randomness
				if (!randomIndices.Contains(rand))
					randomIndices.Add(rand);
            }

			// counter to find which attributes to add
			int counter = 0;

			foreach (var attribute in availableAttributes)
			{
				// if this is an attribute we chose, add it
				if (randomIndices.Contains(counter))
					attributes.Add(attribute.Key);

				// increment the count
				counter++;
			}

			// add our newly created attributes to our attributes text box
			Text textbox = transform.FindChild("Attributes").GetComponent<Text>();
			textbox.text = "";

			foreach (var attribute in attributes)
				textbox.text += attribute + "\n";
		}
	}
}