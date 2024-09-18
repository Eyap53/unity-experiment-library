namespace ExperimentLibrary
{
	using System;
	using System.IO;

	public static class ExperimentUtilities
	{
		/// <summary>
		/// Adds a ".csv" extension to the given file name if it does not already have one.
		/// </summary>
		/// <param name="fileName">The name of the file.</param>
		/// <returns>The file name with a ".csv" extension.</returns>
		/// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or empty.</exception>
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
