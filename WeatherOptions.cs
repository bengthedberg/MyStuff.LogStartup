using System.ComponentModel.DataAnnotations;

public class WeatherOptions
{
    public const string ConfigSectionName = "Weather";

    [Required]
    public int ForecastDays { get; set; }

    public static WeatherOptions LoadAndValidate(ConfigurationManager config)
    {
        var weatherSettings = config.GetSection(ConfigSectionName);
        if (weatherSettings is null || !weatherSettings.Exists())
        {
            throw new ArgumentNullException(nameof(weatherSettings), $"Missing section {ConfigSectionName} in configuration.");
        }
        var days = weatherSettings.GetValue<int>(nameof(ForecastDays));
        if (!(days >= 1 && days <= 14))
        {
            throw new ArgumentOutOfRangeException(nameof(ForecastDays), $"ForcastDays of {days} must be in range 1 to 14.");
        }
        return new WeatherOptions()
        {
            ForecastDays = days
        };
    }

}