namespace ExperimentAppLibrary
{
	using CsvHelper;
	using CsvHelper.Configuration;
	using UnityEngine;
	using System.Linq;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System;

	public class ExperimentOutputs
	{
		public static string GetOutputsFolder()
		{
			//'@' : prevent the escaping of characters with \. Really useful for paths.
			return Path.Combine(@"" + Application.dataPath, "Outputs");
		}

		public static string GetParticipantFolder(int participantId)
		{
			return Path.Combine(GetOutputsFolder(), participantId.ToString());
		}

		/// <summary>
		/// Write a common output data inside the output folder. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file name to write to. The name should NOT include the extension (no .csv).</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteCommonOutput<T>(List<T> records, string fileName) => WriteCommonOutput<T, DefaultClassMap<T>>(records, fileName);
		public static void WriteCommonOutput<T, UMap>(List<T> records, string fileName) where UMap : ClassMap
		{
			if (records is null)
			{
				throw new ArgumentNullException(nameof(records));
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string outputFolder = GetOutputsFolder();
			Directory.CreateDirectory(outputFolder);
			string writePath = Path.Combine(outputFolder, string.Format("{0}.csv", fileName));

			WriteOutput<T, UMap>(records, writePath);
		}

		/// <summary>
		/// Write the participant data inside the output folder.
		/// Note that the data will be overwritten, if any.
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="participantId">The id of the participant.</param>
		/// <param name="fileName">The file name to write to. The name should not includ the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		/// <returns></returns>
		public static void WriteParticipantOutput<T>(List<T> records, int participantId, string fileName) => WriteParticipantOutput<T, DefaultClassMap<T>>(records, participantId, fileName);

		/// <summary>
		/// Write the participant data inside the output folder.
		/// Note that the data will be overwritten, if any.
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="participantId">The id of the participant.</param>
		/// <param name="fileName">The file name to write to. The name should not includ the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		/// <typeparam name="UMap">The classMap type to override default mapping.</typeparam>
		/// <returns></returns>
		public static void WriteParticipantOutput<T, UMap>(List<T> records, int participantId, string fileName) where UMap : ClassMap
		{
			if (records is null)
			{
				throw new ArgumentNullException(nameof(records));
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			Directory.CreateDirectory(participantPath);
			string writePath = Path.Combine(participantPath, string.Format("{0}.csv", fileName));

			WriteOutput<T, UMap>(records, writePath);
		}

		/// <summary>
		/// Write an output data given full path. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file path to write to. The name SHOULD include the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteOutput<T>(List<T> records, string filepath) => WriteOutput<T, DefaultClassMap<T>>(records, filepath);

		/// <summary>
		/// Write an output data given full path. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file path to write to. The name SHOULD include the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteOutput<T, UMap>(List<T> records, string filepath) where UMap : ClassMap
		{
			if (records is null)
			{
				throw new ArgumentNullException(nameof(records));
			}

			if (string.IsNullOrWhiteSpace(filepath))
			{
				throw new ArgumentException($"'{nameof(filepath)}' cannot be null or whitespace.", nameof(filepath));
			}

			using (var writer = new StreamWriter(filepath))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.Context.RegisterClassMap<UMap>();
				csv.WriteRecords(records);
			}
		}

		/// <summary>
		/// Append to existing file the participant data, inside the output folder.
		/// Can be useful to write data when recorded to avoid StackOverflow at the end or losing too much data in case of a Unity crash.
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="participantId">The id of the participant.</param>
		/// <param name="fileName">The file name to write to. The name should not include the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void AppendParticipantOutput<T>(List<T> records, int participantId, string fileName, bool createIfMissing = false) => AppendParticipantOutput<T>(records, participantId, fileName, createIfMissing);
		public static void AppendParticipantOutput<T, UMap>(List<T> records, int participantId, string fileName, bool createIfMissing = false) where UMap : ClassMap
		{
			if (records is null)
			{
				throw new ArgumentNullException(nameof(records));
			}

			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string writePath = Path.Combine(participantPath, string.Format("{0}.csv", fileName));

			if (!File.Exists(writePath))
			{
				if (createIfMissing)
				{
					WriteParticipantOutput<T, UMap>(records, participantId, fileName);
					return;
				}
				else
				{
					throw new ArgumentException("The file does not exist !", fileName);
				}
			}
			else
			{
				var config = new CsvConfiguration(CultureInfo.InvariantCulture)
				{
					// Don't write the header again.
					HasHeaderRecord = false,
				};

				using (var stream = File.Open(writePath, FileMode.Append))
				using (var writer = new StreamWriter(stream))
				using (var csv = new CsvWriter(writer, config))
				{
					csv.Context.RegisterClassMap<UMap>();
					csv.WriteRecords(records);
				}
			}
		}

		public static bool ReadParticipantOutput<T>(int participantId, string fileName, out T[] result) => ReadParticipantOutput<T>(participantId, fileName, out result);
		public static bool ReadParticipantOutput<T, UMap>(int participantId, string fileName, out T[] result) where UMap : ClassMap
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string readPath = Path.Combine(participantPath, string.Format("{0}.csv", fileName));

			if (!File.Exists(readPath))
			{
				result = null;
				return false;
			}
			else
			{
				using (var reader = new StreamReader(readPath))
				using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
				{
					csv.Context.RegisterClassMap<UMap>();
					result = csv.GetRecords<T>().ToArray();
				}
				return true;
			}
		}
	}
}
