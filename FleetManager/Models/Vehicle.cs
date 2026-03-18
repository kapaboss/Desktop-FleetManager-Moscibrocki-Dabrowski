using ReactiveUI;

namespace FleetManager.Models;

public class Vehicle : ReactiveObject
{
    public string Name { get; set; }
    public string LicenseNumber {get; set;}
    public int FuelPercentage {get; set;}
    public string Status {get; set;}
    
}