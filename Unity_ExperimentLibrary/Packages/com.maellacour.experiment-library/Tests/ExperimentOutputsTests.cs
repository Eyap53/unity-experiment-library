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

		[SetUp]
		public void SetUp()
		{
			// Create a test outputs folder before each test
			Directory.CreateDirectory(TestOutputsFolder);
		}

		[TearDown]
		public void TearDown()
		{
			// Delete the test outputs folder after each test
			Directory.Delete(TestOutputsFolder, true);
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
		// 	int participantId = 123;

		// 	// Act
		// 	var participantFolder = ExperimentOutputs.GetParticipantFolder(participantId);

		// 	// Assert
		// 	Assert.IsFalse(string.IsNullOrWhiteSpace(participantFolder));
		// 	Assert.IsTrue(Directory.Exists(participantFolder));
		// }

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

		private class TestClass
		{
			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}
