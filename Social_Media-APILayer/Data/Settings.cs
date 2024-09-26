namespace Social_Media_APILayer.Data
{
	public class Settings
	{
		static public string? ConnectionString { get; private set; }

		static Settings()
		{
			var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
			ConnectionString = config.GetConnectionString("DefaultConnectionString");

		}
	}
}
