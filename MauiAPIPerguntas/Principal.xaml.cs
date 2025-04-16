using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text;
using MauiAPIPerguntas.Models;

using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;

namespace MauiAPIPerguntas;

public partial class Principal : ContentPage
{
    private JsonSerializerOptions _serializerOptions;
    private string MeuEndPoint = "";
    private readonly HttpClient _client = new HttpClient();
    string tkn = "";
    string jsonContent;

    // consumindo a api retorno get
    private readonly ObservableCollection<Pergunta> _perguntas;
    


    public Principal()
	{
		InitializeComponent();
        // mudar o IP para o endereço IP PUBLICO DA VM AZURE
        SessaoLogin.UrlApi = "http://192.168.1.240:4000";

        // get
        _perguntas = new ObservableCollection<Pergunta>();

    }



    // metodo post (endpoint: /pergunta)
    private async Task EnviarPergunta(Pergunta newPergunta)
    {
        string json = JsonSerializer.Serialize<Pergunta>(newPergunta, _serializerOptions);
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
        MeuEndPoint = "/pergunta";
        var response = await _client.PostAsync(SessaoLogin.UrlApi + MeuEndPoint, content);
        if(response.IsSuccessStatusCode)
        {
            lblResponse.Text = "Pergunta enviada com sucesso!";
        }

    }

    private async void btnEnviar_Clicked(object sender, EventArgs e)
    {
        var newPergunta = new Pergunta
        {
            pergunta = txtPergunta.Text,
        };

        await EnviarPergunta(newPergunta);

    }

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        // get

        _perguntas.Clear();

        MeuEndPoint = SessaoLogin.UrlApi + "/pergunta";

        try
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(MeuEndPoint);
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var perguntasResponse = JsonSerializer.Deserialize<RootObject>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (perguntasResponse != null)
                {
                    foreach (var perg in perguntasResponse.Perguntas)
                    {
                        _perguntas.Add(perg); // Fix: Add the entire Pergunta object instead of just the string property
                    }
                }
            }

            lstPerguntas.ItemsSource = _perguntas;

        }
        catch (Exception ex)
        {
            await DisplayAlert("Aviso", "Erro de Comunicação com a API", "OK");
        }
    }
}