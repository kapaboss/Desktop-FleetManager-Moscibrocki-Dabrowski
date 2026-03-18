using System.Linq;
using System.Reactive;
using Avalonia.Controls.ApplicationLifetimes;
using FleetManager.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.ViewModels;

public class EditWindowViewModel : ViewModelBase
{
    private readonly Vehicle _vehicle;
    
    [Reactive] public string Name { get; set; }
    [Reactive] public string LicenseNumber { get; set; }
    [Reactive] public int FuelPercentage { get; set; }
    [Reactive] public string Status { get; set; }
    
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    public ReactiveCommand<Unit, Unit> CancelCommand { get; }

    public EditWindowViewModel(Vehicle vehicle)
    {
        _vehicle = vehicle;
        
        Name = vehicle.Name;
        LicenseNumber = vehicle.LicenseNumber;
        FuelPercentage = vehicle.FuelPercentage;
        Status = vehicle.Status;

        var isValid = this.WhenAnyValue(
            x => x.Name,
            x => x.LicenseNumber,
            x => x.FuelPercentage,
            x => x.Status,
            (name, licenseNumber, fuelPercentage, status) =>
                !string.IsNullOrWhiteSpace(name) && 
                !string.IsNullOrWhiteSpace(licenseNumber) &&
                !string.IsNullOrWhiteSpace(status) &&
                fuelPercentage > 15);
        
        SaveCommand = ReactiveCommand.Create(() =>
        {
            vehicle.Name = Name;
            vehicle.LicenseNumber = LicenseNumber;
            vehicle.FuelPercentage = FuelPercentage;
            vehicle.Status = Status;
            CloseWindow();
        }, isValid);

        CancelCommand = ReactiveCommand.Create(CloseWindow);
        
    }
    
    private void CloseWindow()
    {
        if (App.Current?.ApplicationLifetime is not IClassicDesktopStyleApplicationLifetime desktop) return;
        desktop.Windows.FirstOrDefault(w => w.DataContext == this)?.Close();
    }
    
}