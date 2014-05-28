using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Miner.GameLogic
{
	/// <summary>
	/// Klasa zawierająca właściwości danego obiektu
	/// </summary>
	public class PropertyContainer
	{
		/// <summary>
		/// Słownik właściwości
		/// </summary>
		protected Dictionary<String, object> _properties;

		public PropertyContainer()
		{
			_properties = new Dictionary<String, object>();
		}

		/// <summary>
		/// Aktualizuje właściwość lub jeśli nie ma jej w słowniku - dodaje.
		/// </summary>
		/// <typeparam name="T">Typ właściwości</typeparam>
		/// <param name="name">Nazwa właściwości</param>
		/// <param name="obj">Wartość</param>
		public void UpdateProperty<T>(String name, T obj)
		{
			if (_properties.ContainsKey(name))
				_properties[name] = obj;
			else
				_properties.Add(name, obj);
		}

		/// <summary>
		/// Zwraca właściwość lub domyślną wartość jeśli właściwości nie ma w słowniku
		/// </summary>
		/// <typeparam name="T">Typ właściwości</typeparam>
		/// <param name="name">Nazwa właściwości</param>
		/// <returns>Zwraca wWartość właściwości lub domyślną wartość</returns>
		public T GetProperty<T>(String name)
		{
			if (_properties.ContainsKey(name))
				return (T)_properties[name];

			return default(T);
		}
	}
}
