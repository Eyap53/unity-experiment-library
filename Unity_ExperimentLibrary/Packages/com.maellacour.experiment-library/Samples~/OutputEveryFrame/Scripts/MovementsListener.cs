namespace ExperimentLibrary.Samples.EveryFrame
{
	using System.Collections.Generic;
	using UnityEngine;
	// using UnityAtoms.BaseAtoms;
	using System;
	using System.Globalization;

	public class MovementsListener : MonoBehaviour
	{
		public static readonly string OutputFilename = "exp";

		public static readonly float TimeBetweenWrites = 5f;

		[Header("References Settings")]
		[SerializeField] private int _participantId; // Could be IntReference with Atoms
		[SerializeField] private bool _isGameOnGoing = true; // Could be BoolReference with Atoms

		// [SerializeField] private BoolEvent _isGameOnGoingEvent; // Could be used with Atoms

		[Header("References Data")]
		[SerializeField] private GameObject _gameObjectToTrack;

		private List<MovementOutput> _movementsToWrite;


		protected void Awake()
		{
			// _isGameOnGoingEvent.Register(OnGameOnGoingChanged); // if you use the bool event

			OnGameOnGoingChanged(_isGameOnGoing); // added for this demo
		}

		private void OnGameOnGoingChanged(bool isGameOnGoing)
		{
			if (isGameOnGoing)
			{
				_movementsToWrite = new();
				MovementsWriter.ComputeFilepath(_participantId);
				InvokeRepeating(nameof(WriteMovements), TimeBetweenWrites, TimeBetweenWrites);
			}
			else
			{
				CancelInvoke(nameof(WriteMovements));
				WriteMovements(); // Last write to be sure everything is saved
			}
		}

		protected void Update()
		{
			if (_isGameOnGoing)
			{
				MovementOutput movementToSave = new MovementOutput()
				{
					Time = Time.time,
					RandomValue = UnityEngine.Random.value,
					SomeString = "Some string",
					GameObjectPosition = _gameObjectToTrack.transform.position
				};
				AddMovementToSave(movementToSave);
			}
		}

		public void AddMovementToSave(MovementOutput dataToWrite)
		{
			_movementsToWrite.Add(dataToWrite);
		}

		public void WriteMovements()
		{
			if (_movementsToWrite.Count == 0)
			{
				Debug.LogWarning("MovementsListener: No movements to write");
				return;
			}
			MovementsWriter.WriteMovements(_movementsToWrite);
			_movementsToWrite.Clear();
		}
	}
}
