namespace ExperimentAppLibrary
{
	using System.IO;
	using System.Linq;

	public static class DirectoryExtensions
	{
		/// <summary>
		/// Copy a directory into the destination path. Can be recursive, and can exclude specific extensions.
		/// </summary>
		/// <param name="sourceDirName"></param>
		/// <param name="destDirName"></param>
		/// <param name="copySubDirs"></param>
		/// <param name="extensionExceptions">Prevents files with these extensions from being copied. Do not use dots.</param>
		/// <remarks>
		/// Shameless copy from https://stackoverflow.com/questions/1974019/folder-copy-in-c-sharp.
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
