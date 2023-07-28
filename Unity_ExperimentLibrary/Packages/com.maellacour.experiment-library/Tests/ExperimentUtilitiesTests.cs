namespace ExperimentLibrary.Tests
{
	using NUnit.Framework;
	using System;
	using System.IO;
	using UnityEngine;
	using UnityEngine.TestTools;

	public class ExperimentUtilitiesTests
	{
		[Test]
		public void AddCsvExtension_WithEmptyFileName_ThrowsArgumentException()
		{
			// Arrange
			string fileName = "";

			// Act & Assert
			Assert.Throws<ArgumentException>(() => ExperimentUtilities.AddCsvExtension(fileName));
		}

		[Test]
		public void AddCsvExtension_WithFileNameWithoutExtension_ReturnsFileNameWithCsvExtension()
		{
			// Arrange
			string fileName = "example";
			string expectedFileName = "example.csv";

			// Act
			string actualFileName = ExperimentUtilities.AddCsvExtension(fileName);

			// Assert
			Assert.AreEqual(expectedFileName, actualFileName);
		}

		[Test]
		public void AddCsvExtension_WithFileNameWithCsvExtension_ReturnsSameFileName()
		{
			// Arrange
			string fileName = "data.csv";

			// Act
			string actualFileName = ExperimentUtilities.AddCsvExtension(fileName);

			// Assert
			Assert.AreEqual(fileName, actualFileName);
		}
	}
}
