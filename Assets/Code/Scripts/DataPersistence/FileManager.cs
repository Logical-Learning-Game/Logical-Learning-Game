using System;
using UnityEngine;
using System.IO;

namespace Unity.Game.SaveSystem
{
	public class FileManager
	{
		public static bool WriteToFile(string fileName, string fileContents)
		{
			var fullPath = Path.Combine(Application.persistentDataPath, fileName);

			try
			{
				File.WriteAllText(fullPath, fileContents);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to write to {fullPath} with exception {e}");
				return false;
			}
		}

		public static bool LoadFromFile(string fileName, out string result)
		{
            Debug.Log(Application.persistentDataPath);
            var fullPath = Path.Combine(Application.persistentDataPath, fileName);
			//if (!File.Exists(fullPath))
			//{
			//	Debug.Log("Path is not existed");
			//	File.WriteAllText(fullPath, "");
			//}

			try
			{
				result = File.ReadAllText(fullPath);
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"Failed to read from {fullPath} with exception {e}");
				result = "";
				return false;
			}
		}

	}
}