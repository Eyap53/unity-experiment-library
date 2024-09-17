namespace ExperimentLibrary.Editor
{
	using System.IO;
	using UnityEditor;
	using UnityEditor.Build;
	using UnityEditor.Build.Reporting;
	using UnityEngine;

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
				string parentFolder;
				if (report.summary.platform == BuildTarget.StandaloneWindows ||
					report.summary.platform == BuildTarget.StandaloneWindows64 ||
					report.summary.platform == BuildTarget.StandaloneLinux64
				)
				{
					// Determine the destination folder for the input files.
					string dataFolderName = $"{Path.GetFileNameWithoutExtension(report.summary.outputPath)}_Data";
					parentFolder = Path.Combine(Path.GetDirectoryName(report.summary.outputPath), dataFolderName);
				}
				else if (report.summary.platform == BuildTarget.StandaloneOSX)
				{
					// Determine the destination folder for the input files.
					parentFolder = Path.Combine(report.summary.outputPath, "Contents");
				}
				else
				{
					// Log a warning if the platform is not supported.
					Debug.LogWarning($"ExperimentLibrary: Platform {report.summary.platform} is not supported. Input files will not be copied.");
					return;
				}

				string destinationFolder = Path.Combine(parentFolder, "Inputs");
				// Copy the input files to the destination folder, excluding files with the "meta" extension.
				DirectoryExtensions.Copy(
								sourceDirName: ExperimentInputs.GetInputsFolder(),
								destDirName: destinationFolder,
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
}
