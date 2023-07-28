namespace ExperimentLibrary
{
	using UnityEngine;
	using UnityEngine.Events;

	[CreateAssetMenu(menuName = "ExperimentLibrary/Participant Index", fileName = "ParticipantIndexSO")]
	public class ParticipantIndexSO : ScriptableObject
	{
		[TextArea]
		[SerializeField]
		private string _description;

		private int _participantIndex;

		public int participantIndex
		{
			get => _participantIndex;
			set
			{
				if (_participantIndex != value)
				{
					_participantIndex = value;
					if (OnValueChanged != null)
					{
						OnValueChanged(value);
					}
				}
			}
		}

		public UnityAction<int> OnValueChanged;

		public void Parse(string participantIndexText)
		{
			participantIndex = int.Parse(participantIndexText);
		}

	}
}
