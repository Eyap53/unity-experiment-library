namespace ExperimentLibrary
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
		public static void WriteCommonOutputs<T, UMap>(List<T> records, string fileName) where UMap : ClassMap => WriteCommonOutputs<T>(records, fileName, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Write a common output data inside the output folder. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file name to write to. The name should NOT include the extension (no .csv).</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteCommonOutputs<T>(List<T> records, string fileName, ClassMap map = null)
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
			string writePath = Path.Combine(outputFolder, ExperimentUtilities.AddCsvExtension(fileName));

			WriteOutputs<T>(records, writePath, map);
		}

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
		public static void WriteParticipantOutputs<T, UMap>(List<T> records, int participantId, string fileName) where UMap : ClassMap => WriteParticipantOutputs<T>(records, participantId, fileName, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Write the participant data inside the output folder.
		/// Note that the data will be overwritten, if any.
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="participantId">The id of the participant.</param>
		/// <param name="fileName">The file name to write to. The name should not includ the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		/// <returns></returns>
		public static void WriteParticipantOutputs<T>(List<T> records, int participantId, string fileName, ClassMap map = null)
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
			string writePath = Path.Combine(participantPath, ExperimentUtilities.AddCsvExtension(fileName));

			WriteOutputs<T>(records, writePath, map);
		}

		/// <summary>
		/// Write an output data given full path. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file path to write to. The name SHOULD include the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteOutputs<T, UMap>(List<T> records, string filepath) where UMap : ClassMap => WriteOutputs<T>(records, filepath, ObjectResolver.Current.Resolve<UMap>());

		/// <summary>
		/// Write an output data given full path. Such data can be export of settings, ...
		/// </summary>
		/// <param name="records">The values that needs to be saved. Usually responses from participants.</param>
		/// <param name="fileName">The file path to write to. The name SHOULD include the extension.</param>
		/// <typeparam name="T">The class type of answer.</typeparam>
		public static void WriteOutputs<T>(List<T> records, string filepath, ClassMap map = null)
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
				if (map != null)
				{
					csv.Context.RegisterClassMap(map);
				}
				csv.WriteRecords(records);
			}
		}

		[Obsolete("WriteOutput is deprecated, please use AppendOutput instead.")]
		public static void WriteOutput<T, UMap>(T record, string filepath) where UMap : ClassMap => AppendOutput<T, UMap>(record, filepath);
		[Obsolete("WriteOutput is deprecated, please use AppendOutput instead.")]
		public static void WriteOutput<T>(T record, string filepath, ClassMap map = null) => AppendOutput(record, filepath, map);

		public static void AppendOutput<T, UMap>(T record, string filepath) where UMap : ClassMap => WriteOutput<T>(record, filepath, ObjectResolver.Current.Resolve<UMap>());
		public static void AppendOutput<T>(T record, string filepath, ClassMap map = null)
		{
			if (record is null)
			{
				throw new ArgumentNullException(nameof(record));
			}

			if (string.IsNullOrWhiteSpace(filepath))
			{
				throw new ArgumentException($"'{nameof(filepath)}' cannot be null or whitespace.", nameof(filepath));
			}

			bool fileMissingOrEmpty = !File.Exists(filepath) || new FileInfo(filepath).Length == 0;

			using (var stream = File.Open(writePath, FileMode.Append))
			using (var writer = new StreamWriter(stream))
			using (var csv = new CsvWriter(writer, config))
			{
				if (fileMissingOrEmpty)
				{
					csv.WriteHeader<T>();
					csv.NextRecord();
				}

				if (map != null)
				{
					csv.Context.RegisterClassMap(map);
				}
				csv.WriteRecord(record);
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
		public static void AppendParticipantOutputs<T, UMap>(List<T> records, int participantId, string fileName, bool createIfMissing = false) where UMap : ClassMap => AppendParticipantOutputs<T>(records, participantId, fileName, createIfMissing, ObjectResolver.Current.Resolve<UMap>());
		public static void AppendParticipantOutputs<T>(List<T> records, int participantId, string fileName, bool createIfMissing = false, ClassMap map = null)
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
			string writePath = Path.Combine(participantPath, ExperimentUtilities.AddCsvExtension(fileName));

			if (!File.Exists(writePath))
			{
				if (createIfMissing)
				{
					WriteParticipantOutputs<T>(records, participantId, fileName, map);
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
					if (map != null)
					{
						csv.Context.RegisterClassMap(map);
					}
					csv.WriteRecords(records);
				}
			}
		}

		public static bool ReadParticipantOutputs<T, UMap>(int participantId, string fileName, out T[] result) where UMap : ClassMap => ReadParticipantOutputs<T>(participantId, fileName, out result, ObjectResolver.Current.Resolve<UMap>());
		public static bool ReadParticipantOutputs<T>(int participantId, string fileName, out T[] result, ClassMap map = null)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				throw new ArgumentException($"'{nameof(fileName)}' cannot be null or whitespace.", nameof(fileName));
			}

			string participantPath = GetParticipantFolder(participantId);
			string readPath = Path.Combine(participantPath, ExperimentUtilities.AddCsvExtension(fileName));

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
					if (map != null)
					{
						csv.Context.RegisterClassMap(map);
					}
					result = csv.GetRecords<T>().ToArray();
				}
				return true;
			}
		}
	}
}
