namespace ExperimentLibrary.Converters
{
	using CsvHelper;
	using CsvHelper.Configuration;
	using CsvHelper.TypeConversion;
	using UnityEngine;

	public class Vector2Converter : DefaultTypeConverter
	{
		public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
		{
			return StringToVector2(text);
		}

		public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
		{
			return ((Vector2)value).ToString();
		}

		public static Vector2 StringToVector2(string s)
		{
			string[] temp = s.Substring(1, s.Length - 2).Split(',');
			return new Vector2(float.Parse(temp[0]), float.Parse(temp[1]));
		}
	}
}
