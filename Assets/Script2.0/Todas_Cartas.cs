using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Todas_Cartas : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Game game;
    public List<Cartas> todascartas = new List<Cartas>(); // Inicialização da lista
    [SerializeField] private GameObject CartasPrefab;
    private Dictionary<Player, int> pontuacoes = new Dictionary<Player, int>(); // Dicionário para armazenar pontuações

    public GameObject[] Players;
    public bool primeiraRodada = true;
    public bool deveEsconderCartas;
    public valoresNumeros manilha;
    [SerializeField] private TextMeshPro manilha3DText; // TextMeshPro 3D object para a manilha
    [SerializeField] private TextMeshPro vencedor3DText; // TextMeshPro 3D object para o vencedor
    [SerializeField] private TextMeshPro cartasRestantesText; // TextMeshPro 3D object para mostrar cartas restantes
    [SerializeField] private TextMeshProUGUI pontuacoesText; // TextMeshPro 3D object para mostrar pontuação

    public bool primeiraRodadaTerminou = false;

    private List<Cartas> cartasNaMesa = new List<Cartas>();
    private PlayerAI1 pplayerAI1; // Mantenha apenas esta referência
    public GameObject manilhaText; // Referência para o ManilhaText

    void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager não encontrado na cena.");
        }

        pplayerAI1 = FindObjectOfType<PlayerAI1>();
        if (pplayerAI1 == null)
        {
            Debug.LogError("PlayerAI1 não encontrado na cena!");
        }

        manilhaText = GameObject.Find("ManilhaText");
        if (manilhaText == null)
        {
            Debug.LogError("ManilhaText não encontrado!");
        }

        HashSet<string> cartasAdicionadas = new HashSet<string>();
        naipes[] todosNaipes = (naipes[])Enum.GetValues(typeof(naipes));
        valoresNumeros[] todosValores = (valoresNumeros[])Enum.GetValues(typeof(valoresNumeros));

        // Criar todas as cartas
        foreach (naipes naipe in todosNaipes)
        {
            foreach (valoresNumeros valor in todosValores)
            {
                string idCarta = $"{naipe}{valor}";

                if (!cartasAdicionadas.Contains(idCarta))
                {
                    GameObject cartaObj = Instantiate(CartasPrefab);
                    Cartas carta = cartaObj.GetComponent<Cartas>();
                    carta.naipe = naipe;
                    cartaObj.tag = naipe.ToString();
                    carta.valoresNumeros = valor;
                    todascartas.Add(carta);
                    cartasAdicionadas.Add(idCarta);
                }
            }
        }

        // Inicializar pontuações dos jogadores
        foreach (var player in Players)
        {
            var playerComponent = player.GetComponent<Player>();
            if (playerComponent != null)
            {
                pontuacoes[playerComponent] = 0; // Define a pontuação inicial como 0
            }
        }

        Embaralhar();
        PrimeraMao(); // Certifique-se de que esta função só distribui uma carta por jogador.
        AtualizarTextoCartasRestantes();
        AtualizarTextoPontuacoes(); // Atualiza o texto das pontuações
    }

    public void JogarRodada()
    {
        // Lógica para jogar a rodada...

        // Determine os vencedores (substitua com sua lógica real)
        bool jogadorHumanoGanhou = DeterminarVencedorHumano();
        bool ia1Ganhou = DeterminarVencedorIA1();
        bool ia2Ganhou = DeterminarVencedorIA2();
        bool ia3Ganhou = DeterminarVencedorIA3();

        // Obtenha os nomes dos jogadores
        string nomeHumano = Players[0].GetComponent<Player>().name; // Supondo que o jogador humano é o primeiro na lista
        string nomeIA1 = Players[1].GetComponent<PlayerAI1>().name; // Nome do IA1
        string nomeIA2 = Players[2].GetComponent<PlayerAI1>().name; // Nome do IA2
        string nomeIA3 = Players[3].GetComponent<PlayerAI1>().name; // Nome do IA3

        // Chame o método do ScoreManager para calcular a pontuação
        if (scoreManager != null)
        {
            scoreManager.CalcularPontuacao(jogadorHumanoGanhou, nomeHumano, ia1Ganhou, nomeIA1, ia2Ganhou, nomeIA2, ia3Ganhou, nomeIA3);
        }
    }

    // Métodos para determinar vencedores (você deve implementar)
    private bool DeterminarVencedorHumano() { /*...*/ return false; }
    private bool DeterminarVencedorIA1() { /*...*/ return false; }
    private bool DeterminarVencedorIA2() { /*...*/ return false; }
    private bool DeterminarVencedorIA3() { /*...*/ return false; }

    public void AdicionarCartaNaMesa(Cartas carta)
    {
        cartasNaMesa.Add(carta); // Adiciona a carta à lista de cartas na mesa
        // Lógica adicional, como atualizar a exibição, pode ser adicionada aqui
    }

    public void Embaralhar()
    {
        System.Random rng = new System.Random();
        int n = todascartas.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Cartas temp = todascartas[k];
            todascartas[k] = todascartas[n];
            todascartas[n] = temp;
        }
    }

    public Cartas CartaAleatoria()
    {
        if (todascartas.Count == 0)
        {
            Debug.LogWarning("Não há mais cartas para pegar.");
            return null; // Retorna null se não houver cartas
        }

        int indexAleatorio = UnityEngine.Random.Range(0, todascartas.Count);
        Cartas carta = todascartas[indexAleatorio];
        todascartas.RemoveAt(indexAleatorio); // Remover a carta da lista
        Debug.Log("Carta removida: " + carta.valoresNumeros + " de " + carta.naipe);
        AtualizarTextoCartasRestantes(); // Atualizar o texto das cartas restantes
        return carta;
    }

    public void PrimeraMao()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            GameObject player = Players[i];
            Cartas carta = CartaAleatoria(); // Retira a carta da lista
            if (carta == null) continue; // Se a carta for nula, não prossiga

            // Mova a carta retirada para a posição do jogador
            carta.transform.position = player.transform.position; // Define a posição da carta
            carta.transform.SetParent(player.transform);

            // Adicionar a carta à mão do jogador
            var playerAI = player.GetComponent<PlayerAI1>();
            var playerHumano = player.GetComponent<Player>();
            if (playerAI != null)
            {
                playerAI.hand.Add(carta);
            }
            if (playerHumano != null)
            {
                playerHumano.ListaDeCartas.Add(carta);
            }
        }

        RevelarManilha();
    }

    public void RevelarManilha()
    {
        Cartas cartaVirada = CartaAleatoria();
        if (cartaVirada != null) // Verifica se a carta não é nula
        {
            manilha = DeterminarManilha(cartaVirada.valoresNumeros);
            manilha3DText.text = $"A manilha é: {manilha} (Carta virada: {cartaVirada.valoresNumeros})";

            // Mostrar a carta virada
            GameObject cartaViradaObj = Instantiate(cartaVirada.gameObject);
            cartaViradaObj.transform.position = manilha3DText.transform.position + new Vector3(0, -1, 0);
            cartaViradaObj.transform.SetParent(manilha3DText.transform);
        }
    }

    private valoresNumeros DeterminarManilha(valoresNumeros valorAtual)
    {
        switch (valorAtual)
        {
            case valoresNumeros.quatro: return valoresNumeros.cinco;
            case valoresNumeros.cinco: return valoresNumeros.seis;
            case valoresNumeros.seis: return valoresNumeros.sete;
            case valoresNumeros.sete: return valoresNumeros.oito;
            case valoresNumeros.oito: return valoresNumeros.nove;
            case valoresNumeros.nove: return valoresNumeros.dez;
            case valoresNumeros.dez: return valoresNumeros.valete;
            case valoresNumeros.valete: return valoresNumeros.dama;
            case valoresNumeros.dama: return valoresNumeros.rei;
            case valoresNumeros.rei: return valoresNumeros.ax;
            case valoresNumeros.ax: return valoresNumeros.dois;
            case valoresNumeros.dois: return valoresNumeros.tres;
            case valoresNumeros.tres: return valoresNumeros.quatro; // Volta ao começo
            default: return valoresNumeros.quatro; // Default
        }
    }

    private void AtualizarTextoCartasRestantes()
    {
        if (cartasRestantesText != null)
        {
            cartasRestantesText.text = $"Cartas restantes: {todascartas.Count}";
        }
    }
    public int CalcularForcaDaCarta(Cartas carta)
    {
        // Convertendo os enums para valores inteiros para calcular a força
        int forcaBase = (int)carta.valoresNumeros; // Valor da carta
        int forcaNaipe = (int)carta.naipe; // Naipe da carta

        // Exemplo de fórmula para determinar a força total
        int forcaTotal = forcaBase * 10 + forcaNaipe;

        return forcaTotal; // Retorna a força total da carta
    }
    private void AtualizarTextoPontuacoes()
    {
        if (pontuacoesText != null)
        {
            string pontuacaoText = "Pontuações:\n";
            foreach (var player in pontuacoes)
            {
                pontuacaoText += $"{player.Key.name}: {player.Value}\n";
            }
            pontuacoesText.text = pontuacaoText;
        }
    }
}
