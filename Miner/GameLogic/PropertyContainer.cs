using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameLogic
{
	public class PropertyContainer
	{
		/// <summary>
		/// Dictionary containing properties of any data type.
		/// </summary>
		protected Dictionary<String, object> _properties;

		/// <summary>
		/// Constructor.
		/// </summary>
		public PropertyContainer()
		{
			_properties = new Dictionary<String, object>();
		}

		/// <summary>
		/// Updates value of property if it already exists, otherwise add it to the dictionary.
		/// </summary>
		/// <typeparam name="T">Data type of property to update.</typeparam>
		/// <param name="name">Name of property.</param>
		/// <param name="obj">Object value to store.</param>
		public void UpdateProperty<T>(String name, T obj)
		{
			if (_properties.ContainsKey(name))
				_properties[name] = obj;
			else
				_properties.Add(name, obj);
		}

		/// <summary>
		/// Returns a property by name.
		/// If given key was not found, returns a default value of given data type.
		/// </summary>
		/// <typeparam name="T">Data type of property to retrieve.</typeparam>
		/// <param name="name">Name of property.</param>
		/// <returns>Value of property.</returns>
		public T GetProperty<T>(String name)
		{
			if (_properties.ContainsKey(name))
				return (T)_properties[name];

			return default(T);
		}
	}
}
