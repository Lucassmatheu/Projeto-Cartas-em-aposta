using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Todas_Cartas : MonoBehaviour
{
    public List<Cartas> todascartas;
    [SerializeField] private GameObject CartasPrefab;

    public GameObject[] Players;
    public bool primeiraRodada = true;
    public bool deveEsconderCartas;
    public valoresNumeros manilha;
    [SerializeField] private TextMeshPro manilha3DText; // TextMeshPro 3D object para a manilha
    [SerializeField] private TextMeshPro vencedor3DText; // TextMeshPro 3D object para o vencedor
    [SerializeField] private TextMeshPro cartasRestantesText; // TextMeshPro 3D object para mostrar cartas restantes

    public bool primeiraRodadaTerminou = false;

    private List<Cartas> cartasNaMesa = new List<Cartas>();

    void Awake()
    {
        HashSet<string> cartasAdicionadas = new HashSet<string>();
        naipes[] todosNaipes = (naipes[])Enum.GetValues(typeof(naipes));
        valoresNumeros[] todosValores = (valoresNumeros[])Enum.GetValues(typeof(valoresNumeros));

        foreach (naipes naipe in todosNaipes)
        {
            foreach (valoresNumeros valor in todosValores)
            {
                string idCarta = naipe.ToString() + valor.ToString();

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

        Embaralhar();
        PrimeraMao(); // Certifique-se de que esta função só distribui uma carta por jogador.
        AtualizarTextoCartasRestantes();
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
        int indexAleatorio = UnityEngine.Random.Range(0, todascartas.Count);
        Cartas carta = todascartas[indexAleatorio];
        todascartas.RemoveAt(indexAleatorio); // Remover a carta da lista
        AtualizarTextoCartasRestantes(); // Atualizar o texto das cartas restantes
        return carta;
    }

    public void PrimeraMao()
    {
        for (int i = 0; i < Players.Length; i++)
        {
            GameObject player = Players[i];
            Cartas carta = CartaAleatoria();

            // Instanciar a carta para o jogador
            GameObject cartaObj = Instantiate(CartasPrefab);
            cartaObj.GetComponent<Cartas>().naipe = carta.naipe;
            cartaObj.GetComponent<Cartas>().valoresNumeros = carta.valoresNumeros;

            // Posicionar e associar a carta ao jogador
            cartaObj.transform.position = player.transform.position;
            cartaObj.transform.SetParent(player.transform);

            // Adicionar a carta à mão do jogador
            player.GetComponent<Player>().hand.Add(carta);
        }

        // Revelar a manilha apenas uma vez após a distribuição das cartas
        RevelarManilha();
    }


    public void RevelarManilha()
    {
        Cartas cartaVirada = CartaAleatoria();
        manilha = DeterminarManilha(cartaVirada.valoresNumeros);
        manilha3DText.text = "A manilha é: " + manilha + " (Carta virada: " + cartaVirada.valoresNumeros + ")";

        // Mostrar a carta virada
        GameObject cartaViradaObj = Instantiate(cartaVirada.gameObject);
        cartaViradaObj.transform.position = manilha3DText.transform.position + new Vector3(0, -1, 0);
        cartaViradaObj.transform.SetParent(manilha3DText.transform);
    }

    private valoresNumeros DeterminarManilha(valoresNumeros valorAtual)
    {
        // Lógica de determinação da manilha (mantida a mesma)
        switch (valorAtual)
        {
            case valoresNumeros.quatro: return valoresNumeros.cinco;
            // Outros casos...
            default: return valoresNumeros.quatro;
        }
    }

    public int CalcularForcaDaCarta(Cartas carta)
    {
        int forcaBase = (carta.valoresNumeros == manilha) ? 100 : (int)carta.valoresNumeros;
        int forcaNaipe;

        switch (carta.naipe)
        {
            case naipes.paus:
                forcaNaipe = 50;
                break;
            case naipes.copas:
                forcaNaipe = 35;
                break;
            case naipes.espada:
                forcaNaipe = 20;
                break;
            case naipes.ouro:
                forcaNaipe = 15;
                break;
            default:
                forcaNaipe = 2;
                break;
        }

        return forcaBase * forcaNaipe;
    }

    public Cartas DeterminarCartaVencedora(List<Cartas> cartasNaMesa)
    {
        Cartas cartaVencedora = null;
        int maiorForca = -1;
        int contagemDeCartasComMaiorForca = 0;

        foreach (Cartas carta in cartasNaMesa)
        {
            int forcaDaCarta = CalcularForcaDaCarta(carta);

            if (carta.valoresNumeros == manilha || forcaDaCarta > maiorForca)
            {
                cartaVencedora = carta;
                maiorForca = forcaDaCarta;
                contagemDeCartasComMaiorForca = 1;
            }
            else if (forcaDaCarta == maiorForca && carta.valoresNumeros != manilha)
            {
                contagemDeCartasComMaiorForca++;
            }
        }

        if (contagemDeCartasComMaiorForca > 1)
        {
            Debug.Log("Rodada anulada. Jogando outra rodada...");
            return null;
        }

        return cartaVencedora;
    }

    public void AdicionarCartaNaMesa(Cartas carta, MonoBehaviour jogador)
    {
        cartasNaMesa.Add(carta);

        if (cartasNaMesa.Count == Players.Length)
        {
            Cartas cartaVencedora = DeterminarCartaVencedora(cartasNaMesa);
            if (cartaVencedora != null)
            {
                Player vencedor = DeterminarJogadorVencedor(cartaVencedora);
                if (vencedor != null)
                {
                    vencedor3DText.text = "O vencedor é: " + vencedor.name;
                    MostrarDeclaracoesVencedorPerdedor(vencedor);
                }
            }
            else
            {
                vencedor3DText.text = "Rodada empatada";
                MostrarDeclaracoesVencedorPerdedor(null);
            }

            cartasNaMesa.Clear();
            primeiraRodadaTerminou = true;
        }
    }

    public Player DeterminarJogadorVencedor(Cartas cartaVencedora)
    {
        foreach (var player in Players)
        {
            var playerHand = player.GetComponent<Player>().hand;
            if (playerHand.Contains(cartaVencedora))
            {
                return player.GetComponent<Player>();
            }
        }
        return null;
    }

    public void MostrarDeclaracoesVencedorPerdedor(Player vencedor)
    {
        string declaracoes = "";
        foreach (var player in Players)
        {
            var playerScript = player.GetComponent<Player>();
            if (playerScript == vencedor)
            {
                declaracoes += playerScript.name + " venceu!\n";
            }
            else
            {
                declaracoes += playerScript.name + " perdeu.\n";
            }
        }
        vencedor3DText.text += "\n" + declaracoes;
    }

    public void AtualizarTextoCartasRestantes()
    {
        cartasRestantesText.text = "Cartas restantes: " + todascartas.Count;
    }

    void Update()
    {
        // Lógica de atualização, se necessário.
    }
}
