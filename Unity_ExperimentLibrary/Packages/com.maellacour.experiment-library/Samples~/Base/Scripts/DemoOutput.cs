namespace ExperimentLibrary.Samples.Base
{
	using CsvHelper.Configuration.Attributes;

	/// <summary>
	/// The output for the demo. Nothing much in it, so that it's not complex.
	/// </summary>
	/// <seealso href="https://joshclose.github.io/CsvHelper/examples/writing/write-class-objects/"/>
	/// <seealso href="https://joshclose.github.io/CsvHelper/examples/configuration/attributes/"/>
	public class DemoOutput
	{
		[Name("Identifier")]
		public int Id { get; set; }
		public string Name { get; set; }
		public float RandomValue { get; set; }
	}
}
