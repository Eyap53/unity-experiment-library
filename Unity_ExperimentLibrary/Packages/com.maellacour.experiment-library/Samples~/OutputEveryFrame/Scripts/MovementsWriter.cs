namespace ExperimentLibrary.Samples.EveryFrame
{
	using CsvHelper.TypeConversion;
	using CsvHelper.Configuration;
	using CsvHelper;
	using ExperimentLibrary;
	using ExperimentLibrary.Converters;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using UnityEngine;

	public class MovementsWriter
	{
		public static readonly string OutputFilenameBase = "exp";

		private static string responseOutputFilepath;

		public static void ComputeFilepath(int participantId)
		{
			string participantFolder = ExperimentOutputs.GetParticipantFolder(participantId);
			if (!Directory.Exists(participantFolder))
			{
				Directory.CreateDirectory(participantFolder);
			}
			string datetime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			string filename = $"{OutputFilenameBase}_{participantId}_{datetime}.csv";
			responseOutputFilepath = Path.Combine(participantFolder, filename);
		}

		public static void WriteMovements(List<MovementOutput> _movementsToWrite)
		{
			ExperimentOutputs.WriteOutputs<MovementOutput, MovementOutputMap>(_movementsToWrite, responseOutputFilepath, append: true);
		}
	}

	public class MovementOutputMap : ClassMap<MovementOutput>
	{
		public MovementOutputMap()
		{
			// AutoMap(CultureInfo.InvariantCulture); // NOT WORKING, the Vector3 type is not recognized and causing a bug
			Map(m => m.Time);
			Map(m => m.RandomValue);
			Map(m => m.SomeString);
			Map(m => m.GameObjectPosition).TypeConverter<Vector3Converter>();
		}
	}
}
