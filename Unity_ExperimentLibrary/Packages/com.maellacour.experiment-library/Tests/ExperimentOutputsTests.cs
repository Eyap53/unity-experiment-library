namespace ExperimentLibrary.Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using CsvHelper;
	using CsvHelper.Configuration;
	using ExperimentLibrary;
	using UnityEngine;

	[TestFixture]
	public class ExperimentOutputsTests
	{
		private const string TestOutputsFolder = "./TestOutputs";
		private const int TestParticipantIndex = 999123;
		private const string CommonTestFilename = "common_test";

		[SetUp]
		public void SetUp()
		{
			// Create a test outputs folder before each test
			Directory.CreateDirectory(TestOutputsFolder);

			string participantFolder = ExperimentOutputs.GetParticipantFolder(TestParticipantIndex);
			if (!Directory.Exists(participantFolder))
			{
				Directory.CreateDirectory(participantFolder);
			}
			else
			{
				throw new Exception("Test participant folder already exists");
			}
		}

		[TearDown]
		public void TearDown()
		{
			// Delete the test outputs folder after each test
			Directory.Delete(TestOutputsFolder, true);
			string participantFolder = ExperimentOutputs.GetParticipantFolder(TestParticipantIndex);
			Directory.Delete(participantFolder, true);
		}

		// [Test]
		// public void Test_GetOutputsFolder()
		// {
		// 	// Arrange

		// 	// Act
		// 	var outputsFolder = ExperimentOutputs.GetOutputsFolder();

		// 	// Assert
		// 	Assert.IsFalse(string.IsNullOrWhiteSpace(outputsFolder));
		// 	Assert.IsTrue(Directory.Exists(outputsFolder));
		// }

		// [Test]
		// public void Test_GetParticipantFolder()
		// {
		// 	// Arrange
		// 	int participantId = 999123;

		// 	// Act
		// 	var participantFolder = ExperimentOutputs.GetParticipantFolder(participantId);

		// 	// Assert
		// 	Assert.IsFalse(string.IsNullOrWhiteSpace(participantFolder));
		// 	Assert.IsTrue(Directory.Exists(participantFolder));
		// }

		[Test]
		public void Test_WriteCommonOutputs()
		{
			// Arrange
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" }
			};
			string outputFolder = ExperimentOutputs.GetOutputsFolder();
			string filePath = Path.Combine(outputFolder, ExperimentUtilities.AddCsvExtension(CommonTestFilename));

			// Act
			ExperimentOutputs.WriteCommonOutputs(records, CommonTestFilename);

			// Assert
			Assert.IsTrue(File.Exists(filePath));

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var result = csv.GetRecords<TestClass>().ToList();
				Assert.AreEqual(records.Count, result.Count);
				for (int i = 0; i < records.Count; i++)
				{
					Assert.AreEqual(records[i].Id, result[i].Id);
					Assert.AreEqual(records[i].Name, result[i].Name);
				}
			}

			File.Delete(filePath);
		}

		[Test]
		public void Test_WriteParticipantOutputs()
		{
			// Arrange
			string fileName = "participant_test";
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" }
			};
			string participantFolder = ExperimentOutputs.GetParticipantFolder(TestParticipantIndex);
			string filePath = Path.Combine(participantFolder, ExperimentUtilities.AddCsvExtension(fileName));

			// Act
			ExperimentOutputs.WriteParticipantOutputs(records, TestParticipantIndex, fileName);

			// Assert
			Assert.IsTrue(File.Exists(filePath));

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var result = csv.GetRecords<TestClass>().ToList();
				Assert.AreEqual(records.Count, result.Count);
				for (int i = 0; i < records.Count; i++)
				{
					Assert.AreEqual(records[i].Id, result[i].Id);
					Assert.AreEqual(records[i].Name, result[i].Name);
				}
			}
		}


		[Test]
		public void Test_WriteOutputs()
		{
			// Arrange
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" },
				new TestClass { Id = 3, Name = "Charlie" }
			};
			string filePath = Path.Combine(TestOutputsFolder, "output_test.csv");

			// Act
			ExperimentOutputs.WriteOutputs(records, filePath);

			// Assert
			Assert.IsTrue(File.Exists(filePath));

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var result = csv.GetRecords<TestClass>().ToList();
				Assert.AreEqual(records.Count, result.Count);
				for (int i = 0; i < records.Count; i++)
				{
					Assert.AreEqual(records[i].Id, result[i].Id);
					Assert.AreEqual(records[i].Name, result[i].Name);
				}
			}
		}

		[Test]
		public void Test_AppendOutput()
		{
			// Arrange
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" }
			};
			string filePath = Path.Combine(TestOutputsFolder, "append_test.csv");

			// Act
			foreach (var record in records)
			{
				ExperimentOutputs.AppendOutput(record, filePath);
			}

			// Assert
			Assert.IsTrue(File.Exists(filePath));

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var result = csv.GetRecords<TestClass>().ToList();
				Assert.AreEqual(records.Count, result.Count);
				for (int i = 0; i < records.Count; i++)
				{
					Assert.AreEqual(records[i].Id, result[i].Id);
					Assert.AreEqual(records[i].Name, result[i].Name);
				}
			}
		}

		[Test]
		public void Test_AppendParticipantOutputs()
		{
			// Arrange
			string fileName = "participant_test";
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" }
			};
			string participantFolder = ExperimentOutputs.GetParticipantFolder(TestParticipantIndex);
			string filePath = Path.Combine(participantFolder, ExperimentUtilities.AddCsvExtension(fileName));

			// Act
			ExperimentOutputs.AppendParticipantOutputs(records, TestParticipantIndex, fileName, createIfMissing: true);

			// Assert
			Assert.IsTrue(File.Exists(filePath));

			using (var reader = new StreamReader(filePath))
			using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
			{
				var result = csv.GetRecords<TestClass>().ToList();
				Assert.AreEqual(records.Count, result.Count);
				for (int i = 0; i < records.Count; i++)
				{
					Assert.AreEqual(records[i].Id, result[i].Id);
					Assert.AreEqual(records[i].Name, result[i].Name);
				}
			}
		}

		[Test]
		public void Test_ReadParticipantOutputs()
		{
			// Arrange
			string fileName = "participant_test";
			var records = new List<TestClass>
			{
				new TestClass { Id = 1, Name = "Alice" },
				new TestClass { Id = 2, Name = "Bob" }
			};
			string participantFolder = ExperimentOutputs.GetParticipantFolder(TestParticipantIndex);
			string filePath = Path.Combine(participantFolder, ExperimentUtilities.AddCsvExtension(fileName));

			using (var writer = new StreamWriter(filePath))
			using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
			{
				csv.WriteRecords(records);
			}

			// Act
			bool success = ExperimentOutputs.ReadParticipantOutputs(TestParticipantIndex, fileName, out TestClass[] result);

			// Assert
			Assert.IsTrue(success);
			Assert.IsNotNull(result);
			Assert.AreEqual(records.Count, result.Length);
			for (int i = 0; i < records.Count; i++)
			{
				Assert.AreEqual(records[i].Id, result[i].Id);
				Assert.AreEqual(records[i].Name, result[i].Name);
			}
		}

		[Test]
		public void Test_ReadParticipantOutputs_NonExistentFile()
		{
			// Arrange
			string fileName = "non_existent_file";

			// Act
			bool success = ExperimentOutputs.ReadParticipantOutputs(TestParticipantIndex, fileName, out TestClass[] result);

			// Assert
			Assert.IsFalse(success);
			Assert.IsNull(result);
		}

		private class TestClass
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}