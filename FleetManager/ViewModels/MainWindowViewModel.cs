using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.Json;
using FleetManager.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string FilePath = "Data/vehicles.json";

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public ObservableCollection<Vehicle> Vehicles { get; } = [];
    
    [Reactive] public string NewName { get; set; } = string.Empty;
    [Reactive] public string NewLicenseNumber { get; set; } = string.Empty;
    [Reactive] public int NewFuelPercentage { get; set; } = 0;
    [Reactive] public string NewStatus { get; set; } =  string.Empty;
    
    public ReactiveCommand<Unit, Unit> AddCommand { get; }
    public ReactiveCommand<Unit, Unit> SaveCommand { get; }
    

    public MainWindowViewModel()
    {
        LoadVehicles();

        AddCommand = ReactiveCommand.Create(AddVehicles);
        SaveCommand = ReactiveCommand.Create(SaveToJson);
    }

    private void AddVehicles()
    {
        if (new List<string> { NewName, NewLicenseNumber, NewStatus }.Any(string.IsNullOrWhiteSpace) || NewFuelPercentage <= 0)
        {
            Console.WriteLine("All fields have to be filled correctly.");
            return;
        }
        
        Vehicles.Add(new Vehicle
        {
            Name = NewName,
            LicenceNumber = NewLicenseNumber,
            FuelPercentage = NewFuelPercentage,
            Status = NewStatus
        });
        NewName = NewLicenseNumber = NewStatus = string.Empty;
        NewFuelPercentage = 0;
    }

    private void SaveToJson()
    {
        try
        {
            File.WriteAllText(FilePath, JsonSerializer.Serialize(Vehicles, JsonOptions));
            Console.WriteLine("JSON file saved successfully.");

        }
        catch (Exception exception)
            when (exception is
            IOException or
            UnauthorizedAccessException or
            JsonException)
        {
            Console.WriteLine($"Save File Error {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Unexpected error (report this!): {exception.Message}");
        }
    }

    private void LoadVehicles()
    {
        if(!File.Exists(FilePath)) return;
        try
        {
            var jsonData = File.ReadAllText(FilePath);
            var list = JsonSerializer.Deserialize<List<Vehicle>>(jsonData);
            Vehicles.Clear();
            if(list == null) return;
            foreach (var vehicle in list)
            {
                Vehicles.Add(vehicle);
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}