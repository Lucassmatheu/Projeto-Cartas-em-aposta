using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public List<Cartas> hand = new List<Cartas>();
    [SerializeField] private Transform playerHand;
    [SerializeField] private string playerLayerName = "Player1Card";
    [SerializeField] private Button ganharButton;  // Referência ao GanharButton
    [SerializeField] private Button perderButton;  // Referência ao PerderButton
    private GerenciadorIA gerenciadorIA;
    private MesaManager mesaManager;

    // Declaração das variáveis
    private Cartas cartaaleatoria;
    private GameObject[] _go;
    private bool isFirstRound = true;  // Variável para controlar se é a primeira rodada
    private Todas_Cartas todasCartas;
    private HashSet<Cartas> cartasSelecionadas = new HashSet<Cartas>();
    void Start()
    {
        todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        if (todasCartas == null)
        {
            Debug.LogError("Todas_Cartas não encontrado!");
        }
        mesaManager = GameObject.Find("Mesa").GetComponent<MesaManager>();
        gerenciadorIA = GameObject.FindObjectOfType<GerenciadorIA>();

        if (mesaManager == null)
        {
            Debug.LogError("MesaManager não encontrado!");
        }
        if (gerenciadorIA == null)
        {
            Debug.LogError("GerenciadorIA não encontrado!");
        }

        // Configura os listeners dos botões
        ConfigurarBotoes();

        AdicionarCartaInicial();

        // Se for a primeira rodada, mostrar os botões
        if (isFirstRound)
        {
            MostrarBotoes();
        }
    }

    private void ConfigurarBotoes()
    {
        if (ganharButton == null)
        {
            Debug.LogError("GanharButton não foi atribuído no Inspector!");
        }
        else
        {
            ganharButton.onClick.AddListener(DeclararGanhar);
        }

        if (perderButton == null)
        {
            Debug.LogError("PerderButton não foi atribuído no Inspector!");
        }
        else
        {
            perderButton.onClick.AddListener(DeclararPerder);
        }
    }

    public void DeclararGanhar()
    {
        Declarar(true);  // Chama a função Declarar com o parâmetro true
        EsconderBotoes();  // Esconde os botões após a escolha
        isFirstRound = false;  // Define que não é mais a primeira rodada
    }

    public void DeclararPerder()
    {
        Declarar(false);  // Chama a função Declarar com o parâmetro false
        EsconderBotoes();  // Esconde os botões após a escolha
        isFirstRound = false;  // Define que não é mais a primeira rodada
    }

    private void EsconderBotoes()
    {
        // Desativa os botões para que não possam ser clicados novamente
        ganharButton.gameObject.SetActive(false);
        perderButton.gameObject.SetActive(false);
    }

    private void MostrarBotoes()
    {
        // Ativa os botões para que possam ser clicados
        ganharButton.gameObject.SetActive(true);
        perderButton.gameObject.SetActive(true);
    }

    void AdicionarCartaInicial()
    {
        GameObject cartaPrefab = ObterCartaAleatoria();
        if (cartaPrefab != null)
        {
            // Verifica se a carta já está na mão antes de adicionar
            if (!hand.Contains(cartaPrefab.GetComponent<Cartas>()))
            {
                AddCardToHand(cartaPrefab);
            }
            else
            {
                Debug.LogWarning("A carta já está na mão do jogador.");
            }
        }
        else
        {
            Debug.LogError("Nenhuma carta foi encontrada para adicionar à mão.");
        }
    }
    void RemoverCartasDuplicadas()
    {
        // Um HashSet para controlar as cartas únicas
        HashSet<Cartas> cartasUnicas = new HashSet<Cartas>(hand);
        hand.Clear(); // Limpa a mão

        // Adiciona novamente as cartas únicas
        foreach (var carta in cartasUnicas)
        {
            hand.Add(carta);
        }
    }
    GameObject ObterCartaAleatoria()
    {
        cartaaleatoria = GameObject.Find("ComboDosJogos")?.GetComponent<Todas_Cartas>().CartaAleatoria();
        if (cartaaleatoria == null)
        {
            Debug.LogError("Carta aleatória não foi encontrada!");
            return null;
        }

        // Obtém as cartas do naipe
        _go = GameObject.FindGameObjectsWithTag(cartaaleatoria.naipe.ToString());
        if (_go == null || _go.Length == 0)
        {
            Debug.LogError("Nenhuma carta encontrada com o naipe: " + cartaaleatoria.naipe);
            return null;
        }

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
        if (hand.Contains(cartaComponent))
        {
            Debug.LogWarning("A carta já está na mão do jogador.");
            return; // Não adiciona a carta se já estiver na mão
        }

        // Adiciona a carta se não estiver duplicada
        hand.Add(cartaComponent);

        // Configura a posição e o pai da carta
        cartaObj.transform.position = playerHand.position;
        cartaObj.transform.SetParent(playerHand);

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        SetLayerRecursively(cartaObj, playerLayer);

        Debug.Log("Uma carta foi adicionada à mão do jogador. Agora o jogador tem " + hand.Count + " cartas na mão.");
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public void Declarar(bool vaiGanhar)
    {
        if (gerenciadorIA != null)
        {
            gerenciadorIA.DeclaracaoJogadorHumano(vaiGanhar);
        }
        else
        {
            Debug.LogError("GerenciadorIA não está atribuído no Player.");
        }
    }

    public IEnumerator JogarTurno()
    {
        bool cartaJogada = false;
        while (!cartaJogada)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hand.Count > 0)
                {
                    Cartas cartaParaJogar = hand[0];
                    GameObject cartaObj = cartaParaJogar.gameObject;
                    int slotIndex = SelecionarSlotParaJogar();
                    if (slotIndex >= 0)
                    {
                        mesaManager.ColocarCartaNaMesa(cartaObj, slotIndex, this);
                        hand.Remove(cartaParaJogar);
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
