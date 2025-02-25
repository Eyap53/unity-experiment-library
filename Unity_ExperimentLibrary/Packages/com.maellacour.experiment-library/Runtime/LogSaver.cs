namespace ExperimentLibrary
{
	using System;
	using System.IO;
	using UnityEngine;

	public class LogSaveException : Exception
	{
		public LogSaveException(string message) : base(message) { }
		public LogSaveException(string message, Exception inner) : base(message, inner) { }
	}

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
		/// <returns>True if the logs were saved successfully, false otherwise.</returns>
		public bool SaveLogs(int participantId)
		{
			try
			{
				string logFilePath = ValidateAndGetLogPath();
				string saveLogFilePath = GetUniqueLogFilePath(participantId);

				EnsureDirectoryExists(Path.GetDirectoryName(saveLogFilePath));
				CopyLogFile(logFilePath, saveLogFilePath);

				Debug.Log($"LogSaver: Successfully saved logs to {saveLogFilePath}");
				return true;
			}
			catch (LogSaveException e)
			{
				Debug.LogError($"LogSaver: {e.Message}");
				return false;
			}
			catch (Exception e)
			{
				Debug.LogError("LogSaver: Unexpected error while saving logs!");
				Debug.LogException(e);
				return false;
			}
		}

		private string ValidateAndGetLogPath()
		{
			string logFilePath = Application.consoleLogPath;
			if (string.IsNullOrEmpty(logFilePath))
			{
				throw new LogSaveException("Console log path is null or empty");
			}
			if (!File.Exists(logFilePath))
			{
				throw new LogSaveException($"Console log file not found at {logFilePath}");
			}
			return logFilePath;
		}

		private string GetUniqueLogFilePath(int participantId)
		{
			string timestamp = DateTime.Now.ToString("yy-MM-dd.HH-mm-ss");
			string fileName = $"LogSave.{timestamp}.log";
			string folderPath = ExperimentOutputs.GetParticipantFolder(participantId);
			string filePath = Path.Combine(folderPath, fileName);

			int counter = 1;
			while (File.Exists(filePath))
			{
				fileName = $"LogSave.{timestamp}.{counter++}.log";
				filePath = Path.Combine(folderPath, fileName);
			}

			return filePath;
		}

		private void EnsureDirectoryExists(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				try
				{
					Directory.CreateDirectory(directoryPath);
				}
				catch (Exception e)
				{
					throw new LogSaveException($"Failed to create directory: {directoryPath}", e);
				}
			}
		}

		private void CopyLogFile(string sourcePath, string destinationPath)
		{
			try
			{
				File.Copy(sourcePath, destinationPath);
			}
			catch (Exception e)
			{
				throw new LogSaveException($"Failed to copy log file to {destinationPath}", e);
			}
		}

		/// <summary>
		/// Determines if logs need to be saved and saves them if necessary.
		/// </summary>
		protected abstract void SaveLogsIfNeeded();
	}
}
