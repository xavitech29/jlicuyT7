using jlicuyT7.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace jlicuyT7.Views;


public partial class vEstudiante : ContentPage
{
    private const string url = "http://192.168.0.18/moviles/wsestudiantes.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Estudiante> est;

    public vEstudiante()
    {
        InitializeComponent();
        ObtenerDatos();
    }

    public async Task ObtenerDatos()
    {
        var content = await cliente.GetStringAsync(url);
        List<Estudiante> estudiantes = JsonConvert.DeserializeObject<List<Estudiante>>(content);

        int fila = 1; 
        foreach (var estudiante in estudiantes)
        {
            gridEstudiantes.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var labelCodigo = new Label { Text = estudiante.codigo.ToString() };
            Grid.SetRow(labelCodigo, fila);
            Grid.SetColumn(labelCodigo, 0);
            gridEstudiantes.Children.Add(labelCodigo);

            var labelNombre = new Label { Text = estudiante.nombre };
            Grid.SetRow(labelNombre, fila);
            Grid.SetColumn(labelNombre, 1);
            gridEstudiantes.Children.Add(labelNombre);

            var labelApellido = new Label { Text = estudiante.apellido };
            Grid.SetRow(labelApellido, fila);
            Grid.SetColumn(labelApellido, 2);
            gridEstudiantes.Children.Add(labelApellido);

            var labelEdad = new Label { Text = estudiante.edad.ToString() };
            Grid.SetRow(labelEdad, fila);
            Grid.SetColumn(labelEdad, 3);
            gridEstudiantes.Children.Add(labelEdad);

            
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (sender, e) =>
            {
               
                Estudiante estudianteSeleccionado = estudiante;

                
                await Navigation.PushAsync(new vActEliminar(estudianteSeleccionado));
            };
            labelCodigo.GestureRecognizers.Add(tapGestureRecognizer);
            labelNombre.GestureRecognizers.Add(tapGestureRecognizer);
            labelApellido.GestureRecognizers.Add(tapGestureRecognizer);
            labelEdad.GestureRecognizers.Add(tapGestureRecognizer);

            fila++;
        }
    }

    private async void btnAgregar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new vAgregar());
        await ObtenerDatos();
    }
}