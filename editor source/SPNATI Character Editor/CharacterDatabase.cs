﻿using Desktop;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SPNATI_Character_Editor
{
	/// <summary>
	/// Tracks information across all characters, even those not actively being edited
	/// </summary>
	public static class CharacterDatabase
	{
		public static IEnumerable<Character> Characters
		{
			get { return _characters; }
		}

		public static IEnumerable<Costume> Skins
		{
			get { return _reskins.Values; }
		}

		private static List<Character> _characters = new List<Character>();
		private static Dictionary<string, Character> _characterMap = new Dictionary<string, Character>();
		private static Dictionary<string, Character> _idMap = new Dictionary<string, Character>();
		private static Dictionary<Character, CharacterEditorData> _editorData = new Dictionary<Character, CharacterEditorData>();
		private static Dictionary<string, Costume> _reskins = new Dictionary<string, Costume>();

		public static List<string> FailedCharacters = new List<string>();

		public static int Count
		{
			get { return _characters.Count; }
		}

		public static void Add(Character character)
		{
			_characters.Add(character);
			_characterMap[character.FolderName] = character;
			_idMap[GetId(character)] = character;
		}

		public static string GetId(Character character)
		{
			return GetId(character.FolderName);
		}
		public static string GetId(string id)
		{
			return Regex.Replace(id, @"\W", "");
		}

		public static Character GetRandom()
		{
			return _characters.GetRandom();
		}

		public static Character Get(string folderName)
		{
			return _characterMap.Get(folderName);
		}

		public static Character GetById(string id)
		{
			return _idMap.Get(id);
		}

		public static bool Exists(string folderName)
		{
			return _characterMap.ContainsKey(folderName);
		}

		public static void Set(string folderName, Character character)
		{
			_characterMap[folderName] = character;
			_idMap[GetId(character)] = character;
			for (int i = 0; i < _characters.Count; i++)
			{
				if (_characters[i].FolderName == folderName)
				{
					_characters.RemoveAt(i);
					_characters.Insert(i, character);
					return;
				}
			}
			_characters.Add(character);
		}

		public static void AddEditorData(Character character, CharacterEditorData data)
		{
			data = data ?? new CharacterEditorData();
			data.LinkOwner(character);
			_editorData[character] = data;
			character.Markers.Merge(data.Markers);
		}

		public static CharacterEditorData GetEditorData(Character character)
		{
			if (character == null) { return new CharacterEditorData(); }
			return _editorData.GetOrAddDefault(character, () =>
			{
				CharacterEditorData data = new CharacterEditorData()
				{
					Owner = character.FolderName
				};
				data.LinkOwner(character);
				return data;
			});
		}

		/// <summary>
		/// Record select filter for keeping out humans
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		public static bool FilterHuman(IRecord record)
		{
			return record.Key != "human";
		}

		/// <summary>
		/// Record select filter for keeping out the default
		/// </summary>
		/// <param name="record"></param>
		/// <returns></returns>
		public static bool FilterDefaultCostume(IRecord record)
		{
			return record.Key != "default";
		}

		public static void AddSkin(Costume skin)
		{
			_reskins[skin.Folder] = skin;
		}

		public static Costume GetSkin(string folder)
		{
			return _reskins.Get(folder);
		}
	}
}
