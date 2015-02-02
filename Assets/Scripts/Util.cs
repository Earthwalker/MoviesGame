using System.Collections.Generic;
using System.Linq;

namespace Movies
{
	public static class Util
	{
		/// <summary>
		/// Gets the permutations of a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		public static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items)
		{
			if (items.Count() > 1)
			{
				return items.SelectMany(item => GetPermutations(items.Where(i => !i.Equals(item))),
									   (item, permutation) => new[] { item }.Concat(permutation));
			}
			else
				return new[] { items };
		}
	}
}