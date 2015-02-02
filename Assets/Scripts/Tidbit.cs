namespace Movies
{
	public enum Preference
	{
		none,
		hates,
		dislikes,
		likes,
		loves,
		number
	}

	/// <summary>
	/// Represents a small bit of customer's preferences
	/// </summary>
	[System.Serializable]
	public struct Tidbit
	{
		/// <summary>
		/// The name of the attribute
		/// </summary>
		public string AttributeName;

		/// <summary>
		/// The movie's attributes (i.e. genre or actors)
		/// </summary>
		public string AttributeType;

		/// <summary>
		/// How strongly the attribute is liked or disliked (hate, dislike, like, love)
		/// </summary>
		public Preference AttributePreference;

		/// <summary>
		/// Name of the customer that this tidbit refers to
		/// </summary>
		public string CustomerName;

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="attributeName"></param>
		/// <param name="attributeType"></param>
		/// <param name="preference"></param>
		public Tidbit(string attributeName, string attributeType, Preference preference, string customerName)
		{
			this.AttributeName = attributeName;
			this.AttributeType = attributeType;
			this.AttributePreference = preference;
			this.CustomerName = customerName;
        }

		/// <summary>
		/// Returns whether this tidbit is empty
		/// </summary>
		/// <returns></returns>
		public bool IsEmpty()
		{
			return AttributeName == string.Empty;
		}

		/// <summary>
		/// An empty tidbit
		/// </summary>
		/// <returns></returns>
		public static Tidbit Empty()
		{
			return new Tidbit(string.Empty, string.Empty, Preference.none, string.Empty);
		}
	}
}