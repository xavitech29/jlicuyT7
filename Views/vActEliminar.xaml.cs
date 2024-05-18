using jlicuyT7.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace jlicuyT7.Views;

public partial class vActEliminar : ContentPage
{
    private Estudiante estudiante;
    private const string url = "http://192.168.0.18/moviles/wsestudiantes.php";
    private readonly HttpClient cliente = new HttpClient();

    public vActEliminar(Estudiante estudiante)
    {
        InitializeComponent();
        this.estudiante = estudiante;
        MostrarDetalles();
    }

    private void MostrarDetalles()
    {
        // Muestra los detalles del estudiante
        txtCodigo.Text = estudiante.codigo.ToString();
        txtNombre.Text = estudiante.nombre;
        txtApellido.Text = estudiante.apellido;
        txtEdad.Text = estudiante.edad.ToString();
    }

    private async void btnActualizar_Clicked(object sender, EventArgs e)
    {
   
        estudiante.nombre = txtNombre.Text;
        estudiante.apellido = txtApellido.Text;
        estudiante.edad = Convert.ToInt32(txtEdad.Text);


        var jsonEstudiante = Newtonsoft.Json.JsonConvert.SerializeObject(estudiante);
        var content = new StringContent(jsonEstudiante, System.Text.Encoding.UTF8, "application/json");
        var response = await cliente.PutAsync($"{url}?codigo={estudiante.codigo}", content);

        if (response.IsSuccessStatusCode)
        {
            
            await DisplayAlert("Éxito", "Los detalles del estudiante han sido actualizados correctamente", "OK");

           
            var vistaEstudiante = Navigation.NavigationStack.FirstOrDefault(p => p is vEstudiante) as vEstudiante;
            if (vistaEstudiante != null)
            {
                await vistaEstudiante.ObtenerDatos();
            }
            await Navigation.PushAsync(new vEstudiante());
        }
        else
        {
            // Error al actualizar, muestra un mensaje de error
            await DisplayAlert("Error", "Hubo un problema al actualizar los detalles del estudiante. Por favor, inténtalo de nuevo más tarde", "OK");
        }
    }

    private async void btnEliminar_Clicked(object sender, EventArgs e)
    {
     
        bool confirmacion = await DisplayAlert("Confirmación", "¿Estás seguro que deseas eliminar este estudiante?", "Sí", "No");

        if (confirmacion)
        {
            try
            {
              
                HttpResponseMessage response = await cliente.DeleteAsync($"{url}?codigo={estudiante.codigo}");

                if (response.IsSuccessStatusCode)
                {
                   
                    await DisplayAlert("Éxito", "El estudiante ha sido eliminado correctamente", "OK");

                   
                    var vistaEstudiante = Navigation.NavigationStack.FirstOrDefault(p => p is vEstudiante) as vEstudiante;
                    if (vistaEstudiante != null)
                    {
                        await vistaEstudiante.ObtenerDatos();
                    }

                    
                    await Navigation.PushAsync(new vEstudiante());
                }
                else
                {
                   
                    await DisplayAlert("Error", "Hubo un problema al eliminar el estudiante. Por favor, inténtalo de nuevo más tarde", "OK");
                }
            }
            catch (Exception ex)
            {
                
                await DisplayAlert("Error", $"Hubo un error al intentar eliminar el estudiante: {ex.Message}", "OK");
            }
        }
    }


}