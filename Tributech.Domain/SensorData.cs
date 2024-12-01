namespace Tributech.Domain;

public class SensorData
{
    public string StreamId { get; set; }
    public DateTime Timestamp { get; set; }
    public DateTime StoredAt { get; set; }
    public double Value { get; set; }
}