using System.Net;
using System.Text;
using System.Text.Json;

namespace jlicuyT7.Views;

public partial class vAgregar : ContentPage
{
    private readonly HttpClient _httpClient;

    public vAgregar()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
    }

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        try
        {
           
            var estudiante = new
            {
                nombre = txtNombre.Text,
                apellido = txtApellido.Text,
                edad = txtEdad.Text
            };

          
            var json = JsonSerializer.Serialize(estudiante);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            
            var response = await _httpClient.PostAsync("http://192.168.0.18/moviles/wsestudiantes.php", content);

         
            if (response.IsSuccessStatusCode)
            {
               
                await DisplayAlert("Éxito", "Estudiante agregado correctamente.", "Cerrar");

               
                await Navigation.PushAsync(new vEstudiante());
            }
            else
            {
              
                var errorContent = await response.Content.ReadAsStringAsync();

                
                await DisplayAlert("Error", $"Error al agregar estudiante: {errorContent}", "Cerrar");
            }
        }
        catch (Exception ex)
        {
           
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "Cerrar");
        }
    }
}