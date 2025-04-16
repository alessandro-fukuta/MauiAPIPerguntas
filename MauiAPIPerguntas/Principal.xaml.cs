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

    // post
    private async void btnEnviar_Clicked(object sender, EventArgs e)
    {
        var newPergunta = new Pergunta
        {
            pergunta = txtPergunta.Text,
        };

        await EnviarPergunta(newPergunta);

    }

    //get

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {

        _perguntas.Clear();

        MeuEndPoint = SessaoLogin.UrlApi + "/pergunta";

        try
        {
            // criando o cliente http
            using var httpClient = new HttpClient();
            // configurando o cabeçalho e comunicando com a api
            var response = await httpClient.GetAsync(MeuEndPoint);
            // recebendo o retorno da api e seu codigo de status
            if (response.IsSuccessStatusCode)
            {
                // se o status foi sucesso
                // lendo o conteudo da resposta
                var jsonString = await response.Content.ReadAsStringAsync();
                // deserializando o conteudo
                var perguntasResponse = JsonSerializer.Deserialize<RootObject>(jsonString, new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true }
                );
                // lendo o conteudo deserializado em perguntasResponse e adicionando na lista
                if (perguntasResponse != null)
                {
                    foreach (var perg in perguntasResponse.Perguntas)
                    {
                        // aqui adicionamos o objeto Pergunta inteiro na lista
                        _perguntas.Add(perg); 
                    }
                }
            }

            // finalmente listview recebe a lista de perguntas
            lstPerguntas.ItemsSource = _perguntas;

        }
        catch (Exception ex) // caso ocorra algum erro
        {
            // aqui tratamos o erro
            await DisplayAlert("Erro", "Erro ao buscar perguntas: " + ex.Message, "OK");
        }

    }
}