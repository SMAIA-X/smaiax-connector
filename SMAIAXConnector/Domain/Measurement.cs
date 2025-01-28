using Newtonsoft.Json;

namespace SMAIAXConnector.Domain;

public class Measurement
{
    [JsonProperty("smart_meter_id")] public Guid SmartMeterId { get; set; }
    [JsonProperty("tenant_id")] public Guid TenantId { get; set; }
    [JsonProperty("timestamp")] public DateTime Timestamp { get; set; }
    [JsonProperty("voltage_phase_1")] public double VoltagePhase1 { get; set; }
    [JsonProperty("voltage_phase_2")] public double VoltagePhase2 { get; set; }
    [JsonProperty("voltage_phase_3")] public double VoltagePhase3 { get; set; }
    [JsonProperty("current_phase_1")] public double CurrentPhase1 { get; set; }
    [JsonProperty("current_phase_2")] public double CurrentPhase2 { get; set; }
    [JsonProperty("current_phase_3")] public double CurrentPhase3 { get; set; }

    [JsonProperty("positive_active_power")]
    public double PositiveActivePower { get; set; }

    [JsonProperty("negative_active_power")]
    public double NegativeActivePower { get; set; }

    [JsonProperty("positive_reactive_energy_total")]
    public double PositiveReactiveEnergyTotal { get; set; }

    [JsonProperty("negative_reactive_energy_total")]
    public double NegativeReactiveEnergyTotal { get; set; }

    [JsonProperty("positive_active_energy_total")]
    public double PositiveActiveEnergyTotal { get; set; }

    [JsonProperty("negative_active_energy_total")]
    public double NegativeActiveEnergyTotal { get; set; }

    public override string ToString()
    {
        return $"SmartMeterId: {SmartMeterId},\nTenantId: {TenantId},\nTimestamp: {Timestamp},\n" +
               $"VoltagePhase1: {VoltagePhase1},\nVoltagePhase2: {VoltagePhase2},\nVoltagePhase3: {VoltagePhase3},\n" +
               $"CurrentPhase1: {CurrentPhase1},\nCurrentPhase2: {CurrentPhase2},\nCurrentPhase3: {CurrentPhase3},\n" +
               $"PositiveActivePower: {PositiveActivePower},\nNegativeActivePower: {NegativeActivePower},\n" +
               $"ReactiveEnergyQuadrant1Total: {PositiveReactiveEnergyTotal},\nReactiveEnergyQuadrant3Total: {NegativeReactiveEnergyTotal},\n" +
               $"PositiveActiveEnergyTotal: {PositiveActiveEnergyTotal},\nNegativeActiveEnergyTotal: {NegativeActiveEnergyTotal}";
    }
}