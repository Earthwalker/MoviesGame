using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

namespace Movies
{
	/// <summary>
	/// Handles generation of each new game
	/// 
	/// Attached to Controller
	/// </summary>
	public class Generation : MonoBehaviour
	{
		#region inspector
		public Transform customerGrid;
		public Transform movieGrid;
		public GameObject customerPrefab;
		public GameObject moviePrefab;
		#endregion

		/// <summary>
		/// FileName from which to hold possible names and attributes
		/// </summary>
		string dataFileName = "data.txt";

		/// <summary>
		/// Generates a new game
		/// </summary>
		public void Generate()
		{
			// generation is configured by another component attached to this object
			Config config = GetComponent<Config>();

			// dictionary to hold all attributes
			Dictionary<string, string> attributes = new Dictionary<string, string>();

			// choose to load in a number of each type of attribute to use in the game
			if (!LoadAttributes("Genre", config.GenreTotal, ref attributes) || !LoadAttributes("Actor", config.ActorTotal, ref attributes))
				Debug.LogError("Failed to load attributes");
			else
			{
				// generate customers
				GenerateCustomers(config.CustomerNumber, config.CustomerAttributeNumber, attributes);
				Debug.Log("Generated " + GameObject.FindGameObjectsWithTag("Customer").Length.ToString() + " customers");

				// generates movies to match to our customers
				GenerateMovies(config.MovieNumber, config.MovieAttributeNumber, attributes);
				Debug.Log("Generated " + GameObject.FindGameObjectsWithTag("Movie").Length.ToString() + " movies");
			}
		}

		/// <summary>
		/// Loads a list of attributes from a file
		/// </summary>
		/// <param name="name">Name of attributes to load</param>
		/// <param name="number">Number of attributes to load</param>
		/// <param name="attributes">Attributes loaded</param>
		/// <returns>Whether loading was successful</returns>
		bool LoadAttributes(string name, int number, ref Dictionary<string, string> attributes)
		{
			// fill this with genres we load
			List<string> newAttributes;
			if (!LoadDataSection(name, out newAttributes))
			{
				Debug.LogError("Failed to load " + name + "s");
				return false;
			}

			// since we only want a limited number of attributes, we need to trim our lists down
			while (newAttributes.Count > number)
				newAttributes.RemoveAt(Random.Range(0, newAttributes.Count - 1));

			// once our lists are now under the limits, add them to our dictionary
			foreach (var genre in newAttributes)
				attributes.Add(genre, "Genre");

			// add relevant data to auto fill
			GetComponent<Chat>().autofill.Add(name);
			GetComponent<Chat>().autofill.AddRange(newAttributes);

			Debug.Log("Added " + newAttributes.Count.ToString() + " " + name + "s");

			return attributes.Count > 0;
		}

		/// <summary>
		/// Generate customers with random attributes
		/// </summary>
		/// <param name="number">Number to generate</param>
		/// <param name="attributeNumber">Number of attributes for each customer</param>
		/// <param name="attributes">Available attributes to choose from</param>
		void GenerateCustomers(int number, int attributeNumber, Dictionary<string, string> attributes)
		{
			List<string> names;

			if (!LoadDataSection("Customer Name", out names))
			{
				Debug.LogError("Failed to load customer names");
				return;
			}

			// create a list to fill with our generated customers
			var customers = new List<Customer>();

			for (int i = 0; i < number; i++)
			{
				// create a new customer based on the prefab
				var newCustomer = (GameObject)Instantiate(customerPrefab);

				// set our parent so we get placed in our designated grid
				newCustomer.transform.SetParent(customerGrid);

				// set our toggle group to our parent's
				newCustomer.GetComponent<Toggle>().group = customerGrid.GetComponent<ToggleGroup>();

				// give the customer a random name
				newCustomer.name = names[Random.Range(0, names.Count - 1)];

				// add the new name to the auto fill
				GetComponent<Chat>().autofill.Add(newCustomer.name);

				// set our text box to our new name
				newCustomer.GetComponentInChildren<Text>().text = newCustomer.name;

				// remove the name used so we don't have multiple people with the same name
				names.Remove(newCustomer.name);

				// add a listener to change the current recipient when we click on them
				newCustomer.GetComponent<Toggle>().onValueChanged.AddListener(delegate { if (newCustomer.GetComponent<Toggle>().isOn) GetComponent<Chat>().CurrentRecipient = newCustomer; });

				// add a listener to correctly show current matches
				newCustomer.GetComponent<Toggle>().onValueChanged.AddListener(delegate { if (newCustomer.GetComponent<Toggle>().isOn) GetComponent<Matching>().ShowCustomerMatch(newCustomer.GetComponent<Customer>()); });
				
				// generate tidbits
				newCustomer.GetComponent<Customer>().GenerateTidbits(number, attributes);

				// add our new customer to our list
				customers.Add(newCustomer.GetComponent<Customer>());

				Debug.Log("Created customer: " + newCustomer.name);
			}

			// now that we have a list of all our customers, we can give some data about each other
			foreach (var customer in customers)
				// give each customer a list of all the other ones so they can know some random tidbits about them
				customer.AddTidbits(attributeNumber * GetComponent<Config>().KnowledgeMultiplier * (customers.Count - 1), customers.FindAll(other => !other.name.Equals(customer.name)));
		}

		/// <summary>
		/// Generate movies with random attributes
		/// </summary>
		/// <param name="number">Number to generate</param>
		/// <param name="attributeNumber">Number of attributes for each customer</param>
		/// <param name="attributes">Available attributes to choose from</param>
		void GenerateMovies(int number, int attributeNumber, Dictionary<string, string> attributes)
		{
			List<string> names;

			if (!LoadDataSection("Movie Name", out names))
			{
				Debug.LogError("Failed to load movie names");
				return;
			}

			for (int i = 0; i < number; i++)
			{
				// create a new movie based on the prefab
				var newMovie = (GameObject)Instantiate(moviePrefab);

				// set our parent so we get placed in our designated grid
				newMovie.transform.SetParent(movieGrid);

				// set our toggle group to our parent's
				newMovie.GetComponent<Toggle>().group = movieGrid.GetComponent<ToggleGroup>();

				// give the movie a random name
				newMovie.name = "";
				
				// give a chance to add "The" at the beginning
                if (Random.Range(0, 3) == 0)
					newMovie.name += "The ";

				// pick a random word for the title
				string firstWord = names[Random.Range(0, names.Count - 1)];

				// remove the chosen to word to prevent duplicates
				names.Remove(firstWord);

                newMovie.name += firstWord;

				// give a chance for an additional word
				if (Random.Range(0, 3) == 0)
				{
					// give a chance to add "of"
					if (Random.Range(0, 3) == 0)
						newMovie.name += " of";

					// pick another random word for the title
					string secondWord = names[Random.Range(0, names.Count - 1)];

					// remove the chosen to word to prevent duplicates
					names.Remove(secondWord);

					newMovie.name += " " + secondWord + "s";
				}

				// set our text box to our new name
				newMovie.GetComponentInChildren<Text>().text = newMovie.name;

				// remove the name used so we don't have multiple movies with the same name
				names.Remove(newMovie.name);

				// add a listener to correctly show current matches
				newMovie.GetComponent<Toggle>().onValueChanged.AddListener(delegate { if (newMovie.GetComponent<Toggle>().isOn) GetComponent<Matching>().Match(newMovie.GetComponent<Movie>()); });

				// generate attributes
				newMovie.GetComponent<Movie>().GenerateAttributes(number, attributes);

				Debug.Log("Created movie: " + newMovie.name);
			}
		}

		/// <summary>
		/// Loads a string list of a section from a file
		/// </summary>
		/// <param name="name">Section name to load</param>
		/// <param name="data">Data loaded</param>
		/// <returns>Whether loading was successful</returns>
		bool LoadDataSection(string name, out List<string> data)
		{
			// create our list to be filled
			data = new List<string>();

			// open our data file
			var stream = File.OpenText(dataFileName);

			if (stream == null)
			{
				Debug.LogError("Data file not found");
				return false;
			}

			string line;
			bool reading = false;

			// read through the entire file
			do
			{
				line = stream.ReadLine();

				// if this is the section we are looking for, start reading
				if (line.StartsWith("[" + name + "]"))
					reading = true;
				// brackets signify a new section is starting so we need to stop reading
				else if (line.StartsWith("["))
					reading = false;
				else
				{
					// if this is our section, add in the data
					if (reading && !string.IsNullOrEmpty(line))
						data.Add(line);
				}
			} while (!stream.EndOfStream);

			// close the stream since we're done with it
			stream.Close();

			Debug.Log("Loaded " + data.Count.ToString() + " elements");

			return data.Count > 0;
		}
	}
}
