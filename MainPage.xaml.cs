using System.Collections.ObjectModel;
using System.Text.Json;

namespace proyecto11;

public partial class MainPage : ContentPage
{
    public ObservableCollection<Vehicle> Vehicles { get; set; }

    private List<Vehicle> allVehicles;

    public MainPage()
    {
        InitializeComponent();
        LoadVehicles();
        BindingContext = this;
    }

    private async void LoadVehicles()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("vehiculos.json.txt");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();

        allVehicles = JsonSerializer.Deserialize<List<Vehicle>>(json);

        Vehicles = new ObservableCollection<Vehicle>(allVehicles);
        vehiclesCollectionView.ItemsSource = Vehicles;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        var texto = e.NewTextValue ?? string.Empty;

        if (string.IsNullOrWhiteSpace(texto))
        {
            vehiclesCollectionView.ItemsSource = allVehicles;
            return;
        }

        // Evita NRE si Marca/Tipo son null y compara sin cambiar caso
        var filtrados = allVehicles.Where(v =>
            (v.Marca?.IndexOf(texto, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0 ||
            (v.Tipo?.IndexOf(texto, StringComparison.OrdinalIgnoreCase) ?? -1) >= 0
        ).ToList();

        vehiclesCollectionView.ItemsSource = filtrados;
    }
}
