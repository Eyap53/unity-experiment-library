namespace ExperimentAppLibrary
{
	using CsvHelper;
	using CsvHelper.Configuration;
	using Newtonsoft.Json;
	using System;
	using System.Linq;
	using System.Globalization;
	using System.IO;
	using UnityEngine;

	public class ExperimentInputs
	{
		public static string GetInputsFolder()
		{
			return Path.Combine(@"" + Application.dataPath, "Inputs");
		}

		public static string GetParticipantFolder(int participantId)
		{
			return Path.Combine(GetInputsFolder(), participantId.ToString());
		}

		public static T[] ReadParticipantInput<T, UMap>(int participantId, string fileName) where UMap : ClassMap => ReadParticipantInput<T>(participantId, fileName, ObjectResolver.Current.Resolve<UMap>());
		public static T[] ReadParticipantInput<T>(int participantId, string fileName, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string filePath = Path.Combine(participantPath, string.Format("{0}.csv", fileName));
			return ReadCsvInput<T>(filePath, map);
		}

		public static T[] ReadCommonInput<T, UMap>(string fileName) where UMap : ClassMap => ReadCommonInput<T>(fileName, ObjectResolver.Current.Resolve<UMap>());
		public static T[] ReadCommonInput<T>(string fileName, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string filePath = Path.Combine(GetInputsFolder(), string.Format("{0}.csv", fileName));
			return ReadCsvInput<T>(filePath, map);
		}

		public static T[] ReadCsvInput<T, UMap>(string filePath) where UMap : ClassMap => ReadCsvInput<T>(filePath, ObjectResolver.Current.Resolve<UMap>());
		public static T[] ReadCsvInput<T>(string filePath, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));
			}
			if (!File.Exists(filePath))
			{
				throw new ArgumentException($"File does not exist at '{nameof(filePath)}'.", nameof(filePath));
			}

			T[] result;
			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				if (map != null)
				{
					csv.Context.RegisterClassMap(map);
				}
				result = csv.GetRecords<T>().ToArray();
			}
			return result;
		}

		public static TSettings ReadParticipantSettings<TSettings>(int participantId, string fileName = "Settings")
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string readPath = Path.Combine(participantPath, string.Format("{0}.json", fileName));

			if (!File.Exists(readPath))
			{
				return default(TSettings);
			}
			else
			{
				string json;
				using (StreamReader r = new StreamReader(readPath))
				{
					json = r.ReadToEnd();
					return JsonConvert.DeserializeObject<TSettings>(json);
				}
			}
		}
	}
}
