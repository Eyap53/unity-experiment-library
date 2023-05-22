namespace ExperimentAppLibrary.Editor
{
	using System.IO;
	using UnityEditor.Build;
	using UnityEditor.Build.Reporting;

#if UNITY_EDITOR_WIN || UNITY_EDITOR_LINUX
	public class InputsFileCopy : IPostprocessBuildWithReport
	{
		public int callbackOrder { get { return 0; } }

		public void OnPostprocessBuild(BuildReport report)
		{
			string inputsFolder = ExperimentAppLibrary.ExperimentInputs.GetInputsFolder();
			if (Directory.Exists(inputsFolder))
			{
				string destFolder = Path.Combine(Path.Combine(Path.GetDirectoryName(report.summary.outputPath), string.Format("{0}_Data", Path.GetFileNameWithoutExtension(report.summary.outputPath))), "Inputs");
				DirectoryExtensions.Copy(ExperimentAppLibrary.ExperimentInputs.GetInputsFolder(), destFolder, true, new string[] { "meta" });
			}
		}
	}
#endif
}
