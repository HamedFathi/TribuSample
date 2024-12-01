namespace Tributech.Presentation.Controllers;

public class SensorRequest
{
    public string Name { get; set; }
    public string Location { get; set; }
    public double UpperWarningLimit { get; set; }
    public double LowerWarningLimit { get; set; }
}