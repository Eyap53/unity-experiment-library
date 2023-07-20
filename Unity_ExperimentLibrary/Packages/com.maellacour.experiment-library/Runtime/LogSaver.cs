namespace ExperimentAppLibrary
{
	using System;
	using System.IO;
	using UnityEngine;

	public class LogSaver : MonoBehaviour
	{
		[SerializeField]
		private ParticipantIndexSO _participantIndexSO;

		void OnDestroy()
		{
			// Save the Log inside participant folder.
			string logFilePath = Application.consoleLogPath;
			string saveLogFileName = string.Format("LogSave.{0}.log", DateTime.Now.ToString("yy-MM-dd.HH-mm-ss"));
			string participantFolderPath = ExperimentOutputs.GetParticipantFolder(_participantIndexSO.participantIndex);
			string saveLogFilePath = Path.Combine(participantFolderPath, saveLogFileName);

			try
			{
				Directory.CreateDirectory(participantFolderPath);
				File.Copy(logFilePath, saveLogFilePath);
			}
			catch (System.Exception e)
			{
				Debug.LogError("Couldn't save the logs !");
				Debug.LogException(e);
			}
		}
	}
}
