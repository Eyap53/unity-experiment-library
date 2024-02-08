namespace ExperimentLibrary.Samples.Base
{
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;

	public class DemoWriter : MonoBehaviour
	{
		private List<DemoOutput> _dataToWrite = new List<DemoOutput>();

		protected void Start()
		{
			// Add the data to the list
			_dataToWrite.Add(new DemoOutput { Id = 1, Name = "Alice", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 2, Name = "Bob", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 3, Name = "Charlie", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 4, Name = "David", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 5, Name = "Eve", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 6, Name = "Frank", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 7, Name = "Grace", RandomValue = Random.value });
			_dataToWrite.Add(new DemoOutput { Id = 26, Name = "Zoe", RandomValue = Random.value });

			// Write the outputs
			WriteData();
		}

		public void WriteData()
		{
			string directory = Path.Combine(Application.dataPath, "Outputs");
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}

			string filepath = Path.Combine(directory, "DemoOutput.csv");
			ExperimentOutputs.WriteOutputs(_dataToWrite, filepath);
		}
	}
}
