using Newtonsoft.Json;

namespace SMAIAXConnector.Domain;

public class Measurement
{
    public int Id { get; set; } // Auto-incremented primary key

    public Guid SmartMeterId { get; set; }
    
    public string Uptime { get; set; } = "";
    public DateTime Timestamp { get; set; }
    
    [JsonProperty("1.7.0")] public double PositiveActivePower { get; set; }

    [JsonProperty("1.8.0")] public double PositiveActiveEnergyTotal { get; set; }

    [JsonProperty("2.7.0")] public double NegativeActivePower { get; set; }

    [JsonProperty("2.8.0")] public double NegativeActiveEnergyTotal { get; set; }

    [JsonProperty("3.8.0")] public double ReactiveEnergyQuadrant1Total { get; set; }

    [JsonProperty("4.8.0")] public double ReactiveEnergyQuadrant3Total { get; set; }

    [JsonProperty("16.7.0")] public double TotalPower { get; set; }

    [JsonProperty("31.7.0")] public double CurrentPhase1 { get; set; }

    [JsonProperty("32.7.0")] public double VoltagePhase1 { get; set; }

    [JsonProperty("51.7.0")] public double CurrentPhase2 { get; set; }

    [JsonProperty("52.7.0")] public double VoltagePhase2 { get; set; }

    [JsonProperty("71.7.0")] public double CurrentPhase3 { get; set; }

    [JsonProperty("72.7.0")] public double VoltagePhase3 { get; set; }

    public override string ToString()
    {
        return $"Device ID: {SmartMeterId} \n" + 
               $"Positive Active Power (1.7.0): {PositiveActivePower} W\n" +
               $"Positive Active Energy Total (1.8.0): {PositiveActiveEnergyTotal} Wh\n" +
               $"Negative Active Power (2.7.0): {NegativeActivePower} W\n" +
               $"Negative Active Energy Total (2.8.0): {NegativeActiveEnergyTotal} Wh\n" +
               $"Reactive Energy Quadrant 1 Total (3.8.0): {ReactiveEnergyQuadrant1Total} VArh\n" +
               $"Reactive Energy Quadrant 3 Total (4.8.0): {ReactiveEnergyQuadrant3Total} VArh\n" +
               $"Total Power (16.7.0): {TotalPower} W\n" +
               $"Current Phase 1 (31.7.0): {CurrentPhase1} A\n" +
               $"Voltage Phase 1 (32.7.0): {VoltagePhase1} V\n" +
               $"Current Phase 2 (51.7.0): {CurrentPhase2} A\n" +
               $"Voltage Phase 2 (52.7.0): {VoltagePhase2} V\n" +
               $"Current Phase 3 (71.7.0): {CurrentPhase3} A\n" +
               $"Voltage Phase 3 (72.7.0): {VoltagePhase3} V\n" +
               $"Uptime: {Uptime}\n" +
               $"Timestamp: {Timestamp}";
    }
}