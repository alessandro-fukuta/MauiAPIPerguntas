using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;
using System.Text;
using MauiAPIPerguntas.Models;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MauiAPIPerguntas;

public partial class Principal : ContentPage
{
    private JsonSerializerOptions _serializerOptions;
    private string MeuEndPoint = "";
    private readonly HttpClient _client = new HttpClient();
    string tkn = "";
    string jsonContent;

    public Principal()
	{
		InitializeComponent();
        // mudar o IP para o endereço IP PUBLICO DA VM AZURE
        SessaoLogin.UrlApi = "http://192.168.1.240:4000";
    }

   

    // metodo post (endpoint: /pergunta)
    private async Task EnviarPergunta(Perguntas newPergunta)
    {
        string json = JsonSerializer.Serialize<Perguntas>(newPergunta, _serializerOptions);
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
        var newPergunta = new Perguntas
        {
            pergunta = txtPergunta.Text,
        };

        await EnviarPergunta(newPergunta);

    }
}