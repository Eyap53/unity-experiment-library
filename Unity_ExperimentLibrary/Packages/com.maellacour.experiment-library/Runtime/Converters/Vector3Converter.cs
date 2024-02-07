namespace ExperimentLibrary.Converters
{
	using CsvHelper;
	using CsvHelper.Configuration;
	using CsvHelper.TypeConversion;
	using UnityEngine;

	public class Vector3Converter : DefaultTypeConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			return StringToVector3(text);
		}

		public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
		{
			return ((Vector3)value).ToString();
		}

		public static Vector3 StringToVector3(string s)
		{
			string[] temp = s.Substring(1, s.Length - 2).Split(',');
			return new Vector3(float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]));
		}
	}
}
