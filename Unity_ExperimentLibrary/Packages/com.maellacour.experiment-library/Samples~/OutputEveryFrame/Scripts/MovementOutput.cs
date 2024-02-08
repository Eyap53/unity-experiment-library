namespace ExperimentLibrary.Samples.EveryFrame
{
	using CsvHelper;
	using UnityEngine;

	public class MovementOutput
	{
		public float Time { get; set; }

		public float RandomValue { get; set; }
		public string SomeString { get; set; }
		public Vector3 GameObjectPosition { get; set; }

		public override string ToString()
		{
			return $"Time: {Time}, RandomValue: {RandomValue}, SomeString: {SomeString}, GameObjectPosition: {GameObjectPosition}";
		}
	}
}
