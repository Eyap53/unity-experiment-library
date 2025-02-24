namespace ExperimentLibrary
{
	using System;
	using System.IO;
	using UnityEngine;

	/// <summary>
	/// Abstract base class for saving Unity console logs to participant-specific folders.
	/// </summary>
	public abstract class LogSaver : MonoBehaviour
	{
		protected bool _hasQuit = false;

		/// <summary>
		/// Handles cleanup and log saving when the component is destroyed.
		/// </summary>
		protected virtual void OnDestroy()
		{
			SaveLogsIfNeeded();
		}

		/// <summary>
		/// Handles cleanup and log saving when the application is quitting.
		/// </summary>
		protected virtual void OnApplicationQuit()
		{
			SaveLogsIfNeeded();
			_hasQuit = true;
		}

		/// <summary>
		/// Saves the current Unity log file to the participant's folder with a timestamp.
		/// </summary>
		/// <param name="participantId">The ID of the participant whose folder will contain the log.</param>
		public void SaveLogs(int participantId)
		{
			string logFilePath = Application.consoleLogPath;
			string saveLogFileName = string.Format("LogSave.{0}.log", DateTime.Now.ToString("yy-MM-dd.HH-mm-ss"));
			string participantFolderPath = ExperimentOutputs.GetParticipantFolder(participantId);
			string saveLogFilePath = Path.Combine(participantFolderPath, saveLogFileName);

			try
			{
				Directory.CreateDirectory(participantFolderPath);
				File.Copy(logFilePath, saveLogFilePath);
			}
			catch (System.Exception e)
			{
				Debug.LogError("LogSaver: Couldn't save the logs!");
				Debug.LogException(e);
			}
		}

		/// <summary>
		/// Determines if logs need to be saved and saves them if necessary.
		/// </summary>
		protected abstract void SaveLogsIfNeeded();
	}
}