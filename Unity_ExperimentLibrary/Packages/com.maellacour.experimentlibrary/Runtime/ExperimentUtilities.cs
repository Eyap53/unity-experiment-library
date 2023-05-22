namespace ExperimentAppLibrary
{
	using System;
	using System.IO;

	public static class ExperimentUtilities
    {
		public static string AddCsvExtension(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				throw new ArgumentException("File name cannot be null or empty.");
			}

			string extension = Path.GetExtension(fileName);

			if (string.IsNullOrEmpty(extension))
			{
				return fileName + ".csv";
			}
			else if (extension != ".csv")
			{
				// throw new ArgumentException("File name extension is not .csv.");
				return fileName + ".csv";
			}
			else
			{
				return fileName;
			}
		}
    }
}
