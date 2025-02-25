namespace ExperimentLibrary.Integrations.Atoms.Tests
{
	using NUnit.Framework;
	using UnityEngine;
	using UnityEngine.TestTools;
	using UnityAtoms.BaseAtoms;
	using System.Reflection;
	using System.Collections;
	using System.IO;
	using System.Linq;

	[TestFixture]
	public class AtomLogSaverTests
	{
		private AtomLogSaver _atomLogSaver;
		private IntReference _participantIdReference;
		private FieldInfo _hasQuitField;

		[SetUp]
		public void Setup()
		{
			_atomLogSaver = new GameObject("TestAtomLogSaver").AddComponent<AtomLogSaver>();
			_participantIdReference = new IntReference { Value = 42 };
			_hasQuitField = typeof(LogSaver).GetField("_hasQuit",
				BindingFlags.NonPublic | BindingFlags.Instance);
		}

		[TearDown]
		public void Teardown()
		{
			Object.DestroyImmediate(_atomLogSaver.gameObject);
			DeleteParticipantLogs(42); // Clean up any created log files
		}

		[UnityTest]
		public IEnumerator SaveLogs_WithValidParticipantId_SavesLogs()
		{
			// Arrange
			SetParticipantIdReference(_participantIdReference);

			// Act
			_atomLogSaver.SaveLogs(_participantIdReference.Value);
			yield return null;

			// Assert
			Assert.IsTrue(HasParticipantLogs(42));
		}

		[UnityTest]
		public IEnumerator SaveLogsIfNeeded_WithValidParticipantId_SavesLogs()
		{
			// Arrange
			SetParticipantIdReference(_participantIdReference);
			Assert.IsFalse((bool)_hasQuitField.GetValue(_atomLogSaver));

			// Act
			_atomLogSaver.SendMessage("OnApplicationQuit");
			yield return null;

			// Assert
			Assert.IsTrue((bool)_hasQuitField.GetValue(_atomLogSaver));
			Assert.IsTrue(HasParticipantLogs(42));
		}

		[UnityTest]
		public IEnumerator SaveLogsIfNeeded_WithNullReference_DoesNotSaveLogs()
		{
			// Arrange
			SetParticipantIdReference(null);

			// Act
			_atomLogSaver.SendMessage("OnApplicationQuit");
			yield return null;

			// Assert
			Assert.IsFalse(HasParticipantLogs(42));
		}

		private void SetParticipantIdReference(IntReference reference)
		{
			var field = typeof(AtomLogSaver).GetField("_participantIdReference",
				BindingFlags.NonPublic | BindingFlags.Instance);
			field.SetValue(_atomLogSaver, reference);
		}

		private bool HasParticipantLogs(int participantId)
		{
			string participantFolder = ExperimentOutputs.GetParticipantFolder(participantId);
			return Directory.Exists(participantFolder) &&
				   Directory.GetFiles(participantFolder, "LogSave.*.log").Any();
		}

		private void DeleteParticipantLogs(int participantId)
		{
			string participantFolder = ExperimentOutputs.GetParticipantFolder(participantId);
			if (Directory.Exists(participantFolder))
			{
				Directory.Delete(participantFolder, true);
			}
		}
	}
}
