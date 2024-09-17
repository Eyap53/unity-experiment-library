namespace ExperimentLibrary.Editor
{
	using System.IO;
	using UnityEditor.Build;
	using UnityEditor.Build.Reporting;

#if UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX
	/// <summary>
	/// Handles the copying of input files to the build output directory after the build process.
	/// </summary>
	public class InputsFileCopy : IPostprocessBuildWithReport
	{
		/// <summary>
		/// Gets the callback order for the post-process build step.
		/// </summary>
		public int callbackOrder { get { return 0; } }

		/// <summary>
		/// Called after the build process is complete. Copies the input files to the build output directory.
		/// </summary>
		/// <param name="report">The report containing information about the build.</param>
		public void OnPostprocessBuild(BuildReport report)
		{
			// Get the folder containing the input files.
			string inputsFolder = ExperimentInputs.GetInputsFolder();

			// Only proceed if the input files folder exists and is not empty
			if (Directory.Exists(inputsFolder) && Directory.GetFileSystemEntries(inputsFolder).Length > 0)
			{
				// Determine the destination folder for the input files.
				string folderName = $"{Path.GetFileNameWithoutExtension(report.summary.outputPath)}_Data";
				string destFolder = Path.Combine(Path.Combine(Path.GetDirectoryName(report.summary.outputPath), folderName), "Inputs");

				// Copy the input files to the destination folder, excluding files with the "meta" extension.
				DirectoryExtensions.Copy(
								sourceDirName: ExperimentInputs.GetInputsFolder(),
								destDirName: destFolder,
								copySubDirs: true,
								extensionExceptions: new string[] { "meta" }
				);
			}
			else
			{
				UnityEngine.Debug.Log($"ExperimentLibrary: No input files found in {inputsFolder}. Skipping copy operation.");
			}
		}
	}
#endif
}
