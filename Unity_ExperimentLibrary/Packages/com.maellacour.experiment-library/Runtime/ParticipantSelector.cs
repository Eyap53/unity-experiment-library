namespace ExperimentLibrary
{
	using System.IO;
	using TMPro;
	using UnityEngine;

	public class ParticipantSelector : MonoBehaviour
	{
		[SerializeField]
		protected TMP_InputField _participantInputFieldText;
		[SerializeField]
		protected TextMeshProUGUI _participantFilesText;

		[SerializeField]
		protected ParticipantIndexSO _participantIndexSO;

		protected virtual void OnEnable()
		{
			_participantIndexSO.OnValueChanged += UpdateDisplayParticipantFiles;
		}

		protected virtual void Start()
		{
			_participantInputFieldText.text = _participantIndexSO.participantIndex.ToString();
			UpdateDisplayParticipantFiles();
		}

		protected virtual void OnDisable()
		{
			_participantIndexSO.OnValueChanged -= UpdateDisplayParticipantFiles;
		}

		public virtual void ApplyParticipantIndex(string input)
		{
			if (int.TryParse(input, out int result))
			{
				_participantIndexSO.participantIndex = result;
			}
			else
			{
				// TODO: Display warning
			}
		}

		public virtual int GetParticipantFilesCount()
		{
			string participantPath = ExperimentInputs.GetParticipantFolder(_participantIndexSO.participantIndex);
			if (!Directory.Exists(participantPath))
			{
				return -1;
			}
			else
			{
				return Directory.GetFiles(participantPath, "*", SearchOption.TopDirectoryOnly).Length;
			}
		}

		public virtual void UpdateDisplayParticipantFiles() => UpdateDisplayParticipantFiles(_participantIndexSO.participantIndex);
		public virtual void UpdateDisplayParticipantFiles(int participantId)
		{
			string participantPath = ExperimentInputs.GetParticipantFolder(participantId);
			int participantFilesCount = GetParticipantFilesCount();
			_participantFilesText.color = participantFilesCount > 0 ? Color.white : Color.red;
			_participantFilesText.text = participantFilesCount > 0 ? string.Format("Found {0} file(s).", participantFilesCount) : "No Files";
		}
	}
}
