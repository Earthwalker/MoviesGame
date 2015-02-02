using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	/// <summary>
	/// Handles sending and receiving messages
	/// 
	/// Attached to Controller
	/// </summary>
	public class Chat : MonoBehaviour
	{
		#region inspector

		public GameObject inputTextBox;

		public GameObject outputTextBox;

		public Clock clock;

		#endregion

		/// <summary>
		/// The customer conversation we are viewing/talking with
		/// </summary>
		private GameObject currentRecipient;

		public GameObject CurrentRecipient
		{
			get { return currentRecipient; }
			set
			{
				currentRecipient = value;

				// check if we have any history to show for this customer
				if (chatHistory.ContainsKey(currentRecipient))
					outputTextBox.GetComponent<Text>().text = chatHistory[currentRecipient];
				else
					outputTextBox.GetComponent<Text>().text = string.Empty;
            }
		}


		/// <summary>
		/// Question and response history for each customer
		/// </summary>
		public Dictionary<GameObject, string> chatHistory = new Dictionary<GameObject, string>();

		/// <summary>
		/// Keywords that can be entered
		/// </summary>
		public List<string> autofill;

		/// <summary>
		/// Called at the start of the game
		/// </summary>
		void Start()
		{
			// create our auto fill list
			autofill = new List<string>();

			// add all the preferences as strings to the list
			for (int i = 0; i < (int)Preference.number; i++)
				autofill.Add(((Preference)i).ToString());
        }

		/// <summary>
		/// Called every frame
		/// </summary>
		void Update()
		{
			// get the input text box
			Text inputText = inputTextBox.GetComponentInChildren<Text>();

			// color the text red if it isn't one of our keywords and black if otherwise
			if (autofill.Exists(keyword => keyword.ToLower().StartsWith(inputText.text.ToLower())))
				inputText.color = Color.black;
			else
				inputText.color = Color.red;
		}

		/// <summary>
		/// Called by the submit button
		/// </summary>
		public void Submit()
		{
			SubmitMessage(inputTextBox.GetComponentInChildren<Text>().text);
        }

		/// <summary>
		/// Submits a keyword to generate a response from the current recipient
		/// Accessed by the UI
		/// </summary>
		/// <param name="keyword"></param>
		public void SubmitMessage(string keyword)
		{
			// make sure we are chatting with someone
			if (CurrentRecipient == null)
				return;

			// see if we can find the keyword in the auto fill
			string completedKeyword = autofill.Find(match => match.ToLower().StartsWith(keyword.ToLower()) == true);

			if (completedKeyword != null)
				keyword = completedKeyword;

			// find a tidbit containing the keyword
			Tidbit tidbit = CurrentRecipient.GetComponent<Customer>().GetKnownTidbit(keyword);

			// add our keyword to the history
			AddToHistory("Me: Know anything about " + keyword + "?");

			// build a response and also add it to our history
			AddToHistory(CurrentRecipient.name + ": " + BuildResponse(keyword, tidbit));

			// disable the input box if we have no more turns left
            inputTextBox.SetActive(!clock.Tick());
        }

		/// <summary>
		/// Adds a message to the chat history for the current recipient
		/// </summary>
		/// <param name="message"></param>
		public void AddToHistory(string message)
		{
			// if we don't have any records for this customer yet, create them
			if (!chatHistory.ContainsKey(CurrentRecipient))
				chatHistory.Add(CurrentRecipient, "\n");

			chatHistory[CurrentRecipient] += "\n" + message;
			outputTextBox.GetComponent<Text>().text = chatHistory[CurrentRecipient];
		}

		/// <summary>
		/// Builds a response with the given keyword and tidbit
		/// </summary>
		/// <param name="keyword"></param>
		/// <param name="tidbit"></param>
		/// <returns></returns>
		string BuildResponse(string keyword, Tidbit tidbit)
		{
			// allow a bit of variation when we don't have any information to share
			string[] emptyResponses = new string[] { "No idea", "Nope", "I don't know", "Haven't got a clue", "Not sure about that one" };

			if (keyword == CurrentRecipient.name)
				return "That's me!";
			else if (tidbit.IsEmpty())
				return emptyResponses[Random.Range(0, emptyResponses.Length - 1)];
			else
				return tidbit.CustomerName + " " + tidbit.AttributePreference + " " + tidbit.AttributeName + ".";
		}

		/// <summary>
		/// Changes the current recipient to the next one in the list
		/// </summary>
		public void NextRecipient()
		{
			// create and array of all the customers
			var customers = GameObject.FindGameObjectsWithTag("Customer");
			
			// loop through each customer to find the current recipient
			for (int i = 0; i < customers.Length; i++)
			{
				// once we've found the current recipient, we want the next one
				if (customers[i] == CurrentRecipient)
				{
					if (i < customers.Length - 1)
					{
						CurrentRecipient = customers[i + 1];
						return;
					}
					else
					{
						// allow looping
						CurrentRecipient = customers[0];
						return;
					}
				}
			}
		}

		/// <summary>
		/// Changes the current recipient to the previous one in the list
		/// </summary>
		public void PreviousRecipient()
		{
			// create and array of all the customers
			var customers = GameObject.FindGameObjectsWithTag("Customer");

			// loop through each customer to find the current recipient
			for (int i = 0; i < customers.Length; i++)
			{
				// once we've found the current recipient, we want the previous one
				if (customers[i] == CurrentRecipient)
				{
					if (i > 0)
					{
						CurrentRecipient = customers[i - 1];
						return;
					}
					else
					{
						// allow looping
						CurrentRecipient = customers[customers.Length - 1];
						return;
					}
				}
			}
		}

		/// <summary>
		/// Called when the user selects another user to chat with
		/// </summary>
		public void UpdateCurrentRecipient(GameObject recipient)
		{
			CurrentRecipient = GameObject.FindGameObjectsWithTag("Customer").First(customer => customer.GetComponent<Toggle>().isOn == true);

			// check if we have any history to show for this customer
			if (chatHistory.ContainsKey(CurrentRecipient))
				outputTextBox.GetComponent<Text>().text = chatHistory[CurrentRecipient];
		}
	}
}