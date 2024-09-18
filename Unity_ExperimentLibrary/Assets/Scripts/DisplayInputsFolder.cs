using System.IO;
using TMPro;
using UnityEngine;
using ExperimentLibrary;

public class DisplayInputsFolder : MonoBehaviour
{
	public TMP_Text displayText;

	void Start()
	{
		string folderPath = ExperimentInputs.GetInputsFolder();
		if (Directory.Exists(folderPath))
		{
			int fileCount = Directory.GetFiles(folderPath).Length;
			displayText.text = "Number of files: " + fileCount;
		}
		else
		{
			displayText.text = "Folder not found.";
		}
	}
}
