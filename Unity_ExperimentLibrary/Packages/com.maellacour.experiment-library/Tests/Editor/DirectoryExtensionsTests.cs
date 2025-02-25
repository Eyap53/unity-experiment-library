namespace ExperimentLibrary.Tests
{
	using NUnit.Framework;
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using UnityEngine;
	using UnityEngine.TestTools;

	public class DirectoryExtensionsTests
	{
		// ### ALL PATHS ###
		private static readonly string _folderTestPath = Path.Combine(Application.persistentDataPath, "DirectoryExtensionsTests-ReferenceFolder");
		private static readonly string _fileTestPath = Path.Combine(_folderTestPath, "someFilename.txt");
		private static readonly string _folder2TestPath = Path.Combine(_folderTestPath, "Subfolder1");
		private static readonly string _file2TestPath = Path.Combine(_folder2TestPath, "anotherfile.txt");
		private static readonly string _ignoredTestPath = Path.Combine(_folder2TestPath, "anotherfile.ignored");

		private static readonly string _folderCopyPath = Path.Combine(Application.persistentDataPath, "DirectoryExtensionsTests-CopiedFolder");
		private static readonly string _fileCopyPath = Path.Combine(_folderCopyPath, "someFilename.txt");
		private static readonly string _folder2CopyPath = Path.Combine(_folderCopyPath, "Subfolder1");
		private static readonly string _file2CopyPath = Path.Combine(_folder2CopyPath, "anotherfile.txt");
		private static readonly string _ignoredCopyPath = Path.Combine(_folder2CopyPath, "anotherfile.ignored");


		// A Test behaves as an ordinary method
		[Test]
		public void TestCopySimple()
		{
			PrepareFolders();

			DirectoryExtensions.Copy(_folderTestPath, _folderCopyPath, false);

			Assert.True(Directory.Exists(_folderCopyPath));
			Assert.True(File.Exists(_fileCopyPath));

			CleanUpFolders();
		}

		[Test]
		public void TestCopyRecursive()
		{
			PrepareFolders();

			DirectoryExtensions.Copy(_folderTestPath, _folderCopyPath, true);

			Assert.True(Directory.Exists(_folderCopyPath));
			Assert.True(File.Exists(_fileCopyPath));
			Assert.True(Directory.Exists(_folder2CopyPath));
			Assert.True(File.Exists(_file2CopyPath));

			CleanUpFolders();
		}

		[Test]
		public void TestCopyNotRecursive()
		{
			PrepareFolders();

			DirectoryExtensions.Copy(_folderTestPath, _folderCopyPath, false);

			Assert.True(Directory.Exists(_folderCopyPath));
			Assert.True(File.Exists(_fileCopyPath));
			Assert.False(Directory.Exists(_folder2CopyPath));
			Assert.False(File.Exists(_file2CopyPath));

			CleanUpFolders();
		}

		[Test]
		public void TestCopyIgnoreFile()
		{
			PrepareFolders();

			DirectoryExtensions.Copy(_folderTestPath, _folderCopyPath, true, new[] { "ignored" });

			Assert.False(File.Exists(_ignoredCopyPath));

			CleanUpFolders();
		}


		private void PrepareFolders()
		{
			// ### Safety check ###
			if (Directory.Exists(_folderTestPath))
			{
				Directory.Delete(_folderTestPath, true);
			}
			if (Directory.Exists(_folderCopyPath))
			{
				Directory.Delete(_folderCopyPath, true);
			}

			// ### Test Preparation ###
			Directory.CreateDirectory(_folderTestPath);
			using (File.Create(_fileTestPath))
			{ }
			Directory.CreateDirectory(_folder2TestPath);
			using (File.Create(_file2TestPath))
			{ }
			using (File.Create(_ignoredTestPath))
			{ }

			Assert.True(Directory.Exists(_folderTestPath));
			Assert.True(File.Exists(_fileTestPath));
			Assert.False(Directory.Exists(_folderCopyPath));
			Assert.False(File.Exists(_fileCopyPath));

			Assert.True(Directory.Exists(_folder2TestPath));
			Assert.True(File.Exists(_file2TestPath));
			Assert.False(Directory.Exists(_folder2CopyPath));
			Assert.False(File.Exists(_file2CopyPath));

			Assert.True(File.Exists(_ignoredTestPath));
			Assert.False(File.Exists(_ignoredCopyPath));
		}

		private void CleanUpFolders()
		{
			Directory.Delete(_folderTestPath, true);
			Directory.Delete(_folderCopyPath, true);
		}
	}
}
