namespace ExperimentLibrary
{
	using System.IO;
	using System.Linq;

	/// <summary>
	/// Provides extension methods for directory operations.
	/// </summary>
	public static class DirectoryExtensions
	{
		/// <summary>
		/// Copies a directory to a specified destination path. The copy can be recursive and can exclude files with specific extensions.
		/// </summary>
		/// <param name="sourceDirName">The path of the directory to copy.</param>
		/// <param name="destDirName">The path of the destination directory.</param>
		/// <param name="copySubDirs">If true, copies subdirectories recursively.</param>
		/// <param name="extensionExceptions">An array of file extensions to exclude from copying. Do not include dots in the extensions.</param>
		/// <exception cref="DirectoryNotFoundException">Thrown when the source directory does not exist or could not be found.</exception>
		/// <remarks>
		/// This method is a modified version of a solution found at https://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp.
		/// </remarks>
		public static void Copy(string sourceDirName, string destDirName, bool copySubDirs, string[] extensionExceptions = null)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			// If the destination directory does not exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}


			// Get the file contents of the directory to copy.
			FileInfo[] files = dir.GetFiles();

			foreach (FileInfo file in files)
			{
				if (extensionExceptions == null || !extensionExceptions.Contains(file.Extension.TrimStart('.')))
				{
					// Create the path to the new copy of the file.
					string temppath = Path.Combine(destDirName, file.Name);

					// Copy the file.
					file.CopyTo(temppath, true); // OVERWRITE IF EXIST ALREADY
				}
			}

			// If copySubDirs is true, copy the subdirectories.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					// Create the subdirectory.
					string temppath = Path.Combine(destDirName, subdir.Name);

					// Copy the subdirectories.
					Copy(subdir.FullName, temppath, copySubDirs, extensionExceptions);
				}
			}
		}
	}
}
