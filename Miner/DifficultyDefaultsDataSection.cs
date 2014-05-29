using System;
using System.Configuration;

namespace Miner
{
	public class DifficultyDefaultsDataSection : ConfigurationSection
	{
		public const string SectionName = "DifficultyDefaults";
 
		private const string LevelDefaultsCollectionName = "DifficultyLevelDefaults";
 
		[ConfigurationProperty(LevelDefaultsCollectionName)]
		[ConfigurationCollection(typeof(DifficultyLevelDefaultsCollection), AddItemName = "add")]
		public DifficultyLevelDefaultsCollection DifficultyLevelDefaults { get { return (DifficultyLevelDefaultsCollection)base[LevelDefaultsCollectionName]; } }
	}

	public class DifficultyLevelDefaultsCollection : ConfigurationElementCollection
	{
		protected override ConfigurationElement CreateNewElement()
		{
			return new DifficultyLevelDefaultsElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((DifficultyLevelDefaultsElement)element).LevelName;
		}
	}

	public class DifficultyLevelDefaultsElement : ConfigurationElement
	{
		[ConfigurationProperty("levelName", IsRequired = true)]
		public String LevelName
		{
			get { return (String)this["levelName"]; }
			set { this["levelName"] = value; }
		}

		[ConfigurationProperty("maxOxygen", IsRequired = true)]
		public int MaxOxygen
		{
			get
			{
				return (int)this["maxOxygen"];
			}
			set
			{ this["maxOxygen"] = value; }
		}

		[ConfigurationProperty("startLives", IsRequired = true)]
		public int StartLives
		{
			get
			{
				return (int)this["startLives"];
			}
			set
			{ this["startLives"] = value; }
		}

		[ConfigurationProperty("startDynamite", IsRequired = true)]
		public int StartDynamite
		{
			get
			{
				return (int)this["startDynamite"];
			}
			set
			{ this["startDynamite"] = value; }
		}
	}
}