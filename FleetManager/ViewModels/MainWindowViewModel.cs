using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive.Linq;
using System.Text.Json;
using FleetManager.Models;

namespace FleetManager.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private const string FilePath = "Data/vehicles.json";

    public ObservableCollection<Vehicle> Vehicles { get; } = [];

    public MainWindowViewModel()
    {
        LoadVehicles();
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