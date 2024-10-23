using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Cartas> ListaDeCartas = new List<Cartas>();
    [SerializeField] private Transform playerHand;
    [SerializeField] private string playerLayerName = "Player1Card";
    [SerializeField] private Button ganharButton;  // Referência ao GanharButton
    [SerializeField] private Button perderButton;  // Referência ao PerderButton
    private GerenciadorIA gerenciadorIA;
    private MesaManager mesaManager;

    private Cartas cartaaleatoria;
    private GameObject[] _go;
    private bool isFirstRound = true;  // Variável para controlar se é a primeira rodada
    private Todas_Cartas todasCartas;
    private HashSet<Cartas> cartasSelecionadas = new HashSet<Cartas>();

    void Start()
    {
        todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        mesaManager = GameObject.Find("Mesa").GetComponent<MesaManager>();
        gerenciadorIA = GameObject.FindObjectOfType<GerenciadorIA>();

        // Verifica se os componentes estão atribuídos
        if (todasCartas == null) Debug.LogError("Todas_Cartas não encontrado!");
        if (mesaManager == null) Debug.LogError("MesaManager não encontrado!");
        if (gerenciadorIA == null) Debug.LogError("GerenciadorIA não encontrado!");

        // Configura os listeners dos botões
        ConfigurarBotoes();

        // Adiciona uma carta inicial ao jogador
        //AdicionarUmaCartaInicial();

        // Se for a primeira rodada, mostrar os botões
        if (isFirstRound)
        {
            MostrarBotoes();
        }
    }

    private void ConfigurarBotoes()
    {
        ganharButton.onClick.AddListener(DeclararGanhar);
        perderButton.onClick.AddListener(DeclararPerder);
    }

    public void DeclararGanhar()
    {
        Declarar(true);
        EsconderBotoes();
        isFirstRound = false;
    }

    public void DeclararPerder()
    {
        Declarar(false);
        EsconderBotoes();
        isFirstRound = false;
    }

    private void EsconderBotoes()
    {
        ganharButton.gameObject.SetActive(false);
        perderButton.gameObject.SetActive(false);
    }

    private void MostrarBotoes()
    {
        ganharButton.gameObject.SetActive(true);
        perderButton.gameObject.SetActive(true);
    }

    void AdicionarUmaCartaInicial()
    {
        if (ListaDeCartas.Count == 0)
        {
            GameObject cartaPrefab = ObterCartaAleatoria();
            if (cartaPrefab != null)
            {
                AddCardToHand(cartaPrefab);
            }
            else
            {
                Debug.LogError("Nenhuma carta foi encontrada para adicionar à mão.");
            }
        }
    }


    GameObject ObterCartaAleatoria()
    {
        cartaaleatoria = todasCartas.CartaAleatoria();
        if (cartaaleatoria == null)
        {
            Debug.LogError("Carta aleatória não foi encontrada!");
            return null;
        }

        // Remova a carta selecionada da lista de cartas disponíveis
        todasCartas.todascartas.Remove(cartaaleatoria);

        _go = GameObject.FindGameObjectsWithTag(cartaaleatoria.naipe.ToString());
        foreach (var carta in _go)
        {
            var cartaComponent = carta.GetComponent<Cartas>();
            if (!cartasSelecionadas.Contains(cartaComponent) && cartaComponent.valoresNumeros == cartaaleatoria.valoresNumeros)
            {
                cartasSelecionadas.Add(cartaComponent); // Marca a carta como selecionada
                return carta; // Retorna a carta se ainda não foi selecionada
            }
        }

        Debug.LogError("Nenhuma carta correspondente foi encontrada ou já foi selecionada.");
        return null;
    }
    void AddCardToHand(GameObject cartaObj)
    {
        if (cartaObj == null)
        {
            Debug.LogError("A carta a ser adicionada é nula.");
            return;
        }

        var cartaComponent = cartaObj.GetComponent<Cartas>();
        if (cartaComponent == null)
        {
            Debug.LogError("O GameObject não tem o componente Cartas.");
            return;
        }

        // Verifica se a carta já está na mão
        if (ListaDeCartas.Contains(cartaComponent))
        {
            Debug.LogWarning("A carta já está na mão do jogador.");
            return;
        }

        // Adiciona a carta à mão
        ListaDeCartas.Add(cartaComponent);

        // Configura a posição e o pai da carta somente se a carta foi adicionada à mão
        cartaObj.transform.position = playerHand.position;
        cartaObj.transform.SetParent(playerHand);

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        SetLayerRecursively(cartaObj, playerLayer);

        Debug.Log("Uma carta foi adicionada à mão do jogador. Agora o jogador tem " + ListaDeCartas.Count + " cartas na mão.");
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void Declarar(bool vaiGanhar)
    {
        gerenciadorIA?.DeclaracaoJogadorHumano(vaiGanhar);
    }

    public IEnumerator JogarTurno()
    {
        bool cartaJogada = false;
        while (!cartaJogada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (ListaDeCartas.Count > 0)
                {
                    Cartas cartaParaJogar = ListaDeCartas[0]; // Seleciona a primeira carta na mão
                    GameObject cartaObj = cartaParaJogar.gameObject;
                    int slotIndex = SelecionarSlotParaJogar();
                    if (slotIndex >= 0)
                    {
                        mesaManager.ColocarCartaNaMesa(cartaObj, slotIndex, this);
                        ListaDeCartas.Remove(cartaParaJogar);
                        Debug.Log("Carta jogada: " + cartaParaJogar.valoresNumeros + " de " + cartaParaJogar.naipe);
                        cartaJogada = true;
                    }
                }
            }
            yield return null;
        }
    }

    int SelecionarSlotParaJogar()
    {
        for (int i = 0; i < mesaManager.slots.Length; i++)
        {
            if (mesaManager.slots[i].childCount == 0)
            {
                return i;
            }
        }

        Debug.LogError("Nenhum slot disponível para jogar a carta.");
        return -1;
    }
}
