namespace ExperimentLibrary
{
	using CsvHelper;
	using CsvHelper.Configuration;
	using Newtonsoft.Json;
	using System;
	using System.Linq;
	using System.Globalization;
	using System.IO;
	using UnityEngine;

	/// <summary>
	/// Provides methods to handle experiment input files, including reading participant-specific and common input files.
	/// </summary>
	public class ExperimentInputs
	{
		/// <summary>
		/// Gets the path to the inputs folder.
		/// </summary>
		/// <returns>The path to the inputs folder.</returns>
		public static string GetInputsFolder()
		{
			return Path.Combine(@"" + Application.streamingAssetsPath, "Inputs");
		}

		/// <summary>
		/// Gets the path to the folder for a specific participant.
		/// </summary>
		/// <param name="participantId">The ID of the participant.</param>
		/// <returns>The path to the participant's folder.</returns>
		public static string GetParticipantFolder(int participantId)
		{
			return Path.Combine(GetInputsFolder(), participantId.ToString());
		}

		/// <summary>
		/// Reads the input file for a specific participant and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <typeparam name="UMap">The class map type for CSV mapping.</typeparam>
		/// <param name="participantId">The ID of the participant.</param>
		/// <param name="fileName">The name of the input file.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		public static T[] ReadParticipantInput<T, UMap>(int participantId, string fileName) where UMap : ClassMap => ReadParticipantInput<T>(participantId, fileName, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Reads the input file for a specific participant and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <param name="participantId">The ID of the participant.</param>
		/// <param name="fileName">The name of the input file.</param>
		/// <param name="map">The class map for CSV mapping. If null, default mapping is used.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or whitespace.</exception>
		public static T[] ReadParticipantInput<T>(int participantId, string fileName, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string filePath = Path.Combine(participantPath, ExperimentUtilities.AddCsvExtension(fileName));
			return ReadCsvInput<T>(filePath, map);
		}

		/// <summary>
		/// Reads a common input file and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <typeparam name="UMap">The class map type for CSV mapping.</typeparam>
		/// <param name="fileName">The name of the input file.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		public static T[] ReadCommonInput<T, UMap>(string fileName) where UMap : ClassMap => ReadCommonInput<T>(fileName, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Reads a common input file and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <param name="fileName">The name of the input file.</param>
		/// <param name="map">The class map for CSV mapping. If null, default mapping is used.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or whitespace.</exception>
		public static T[] ReadCommonInput<T>(string fileName, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string filePath = Path.Combine(GetInputsFolder(), ExperimentUtilities.AddCsvExtension(fileName));
			return ReadCsvInput<T>(filePath, map);
		}

		/// <summary>
		/// Reads a CSV input file and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <typeparam name="UMap">The class map type for CSV mapping.</typeparam>
		/// <param name="filePath">The path to the CSV file.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is null, whitespace, or the file does not exist.</exception>
		public static T[] ReadCsvInput<T, UMap>(string filePath) where UMap : ClassMap => ReadCsvInput<T>(filePath, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Reads a CSV input file and maps it to an array of objects of type <typeparamref name="T"/>.
		/// </summary>
		/// <typeparam name="T">The type of objects to map the CSV data to.</typeparam>
		/// <param name="filePath">The path to the CSV file.</param>
		/// <param name="map">The class map for CSV mapping. If null, default mapping is used.</param>
		/// <returns>An array of objects of type <typeparamref name="T"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="filePath"/> is null, whitespace, or the file does not exist.</exception>
		public static T[] ReadCsvInput<T>(string filePath, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException($"'{nameof(filePath)}' cannot be null or whitespace.", nameof(filePath));
			}
			if (!File.Exists(filePath))
			{
				throw new ArgumentException($"File does not exist at '{filePath}'.", nameof(filePath));
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

		/// <summary>
		/// Reads the settings file for a specific participant and maps it to an object of type <typeparamref name="TSettings"/>.
		/// </summary>
		/// <typeparam name="TSettings">The type of the settings object.</typeparam>
		/// <param name="participantId">The ID of the participant.</param>
		/// <param name="fileName">The name of the settings file. Defaults to "Settings".</param>
		/// <returns>An object of type <typeparamref name="TSettings"/> if the file exists; otherwise, the default value of <typeparamref name="TSettings"/>.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or whitespace.</exception>
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
