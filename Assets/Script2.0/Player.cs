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
    [SerializeField] private Button ganharButton;  // Refer�ncia ao GanharButton
    [SerializeField] private Button perderButton;  // Refer�ncia ao PerderButton
    private GerenciadorIA gerenciadorIA;
    private MesaManager mesaManager;

    // Declara��o das vari�veis
    private Cartas cartaaleatoria;
    private GameObject[] _go;
    private bool isFirstRound = true;  // Vari�vel para controlar se � a primeira rodada

    void Start()
    {
        mesaManager = GameObject.Find("Mesa").GetComponent<MesaManager>();
        gerenciadorIA = GameObject.FindObjectOfType<GerenciadorIA>();

        if (mesaManager == null)
        {
            Debug.LogError("MesaManager n�o encontrado!");
        }
        if (gerenciadorIA == null)
        {
            Debug.LogError("GerenciadorIA n�o encontrado!");
        }

        // Configura os listeners dos bot�es
        ConfigurarBotoes();

        AdicionarCartaInicial();

        // Se for a primeira rodada, mostrar os bot�es
        if (isFirstRound)
        {
            MostrarBotoes();
        }
    }

    private void ConfigurarBotoes()
    {
        if (ganharButton == null)
        {
            Debug.LogError("GanharButton n�o foi atribu�do no Inspector!");
        }
        else
        {
            ganharButton.onClick.AddListener(DeclararGanhar);
        }

        if (perderButton == null)
        {
            Debug.LogError("PerderButton n�o foi atribu�do no Inspector!");
        }
        else
        {
            perderButton.onClick.AddListener(DeclararPerder);
        }
    }

    public void DeclararGanhar()
    {
        Declarar(true);  // Chama a fun��o Declarar com o par�metro true
        EsconderBotoes();  // Esconde os bot�es ap�s a escolha
        isFirstRound = false;  // Define que n�o � mais a primeira rodada
    }

    public void DeclararPerder()
    {
        Declarar(false);  // Chama a fun��o Declarar com o par�metro false
        EsconderBotoes();  // Esconde os bot�es ap�s a escolha
        isFirstRound = false;  // Define que n�o � mais a primeira rodada
    }

    private void EsconderBotoes()
    {
        // Desativa os bot�es para que n�o possam ser clicados novamente
        ganharButton.gameObject.SetActive(false);
        perderButton.gameObject.SetActive(false);
    }

    private void MostrarBotoes()
    {
        // Ativa os bot�es para que possam ser clicados
        ganharButton.gameObject.SetActive(true);
        perderButton.gameObject.SetActive(true);
    }

    void AdicionarCartaInicial()
    {
        GameObject cartaPrefab = ObterCartaAleatoria();
        if (cartaPrefab != null)
        {
            AddCardToHand(cartaPrefab);
        }
        else
        {
            Debug.LogError("Nenhuma carta foi encontrada para adicionar � m�o.");
        }
    }

    GameObject ObterCartaAleatoria()
    {
        cartaaleatoria = GameObject.Find("ComboDosJogos")?.GetComponent<Todas_Cartas>().CartaAleatoria();
        if (cartaaleatoria == null)
        {
            Debug.LogError("Carta aleat�ria n�o foi encontrada!");
            return null;
        }

        _go = GameObject.FindGameObjectsWithTag(cartaaleatoria.naipe.ToString());
        if (_go == null || _go.Length == 0)
        {
            Debug.LogError("Nenhuma carta encontrada com o naipe: " + cartaaleatoria.naipe);
            return null;
        }

        foreach (var carta in _go)
        {
            if (carta.GetComponent<Cartas>().valoresNumeros == cartaaleatoria.valoresNumeros)
            {
                return carta;
            }
        }

        Debug.LogError("Nenhuma carta correspondente foi encontrada.");
        return null;
    }

    void AddCardToHand(GameObject cartaObj)
    {
        if (cartaObj == null)
        {
            Debug.LogError("A carta a ser adicionada � nula.");
            return;
        }

        var cartaComponent = cartaObj.GetComponent<Cartas>();
        if (cartaComponent == null)
        {
            Debug.LogError("O GameObject n�o tem o componente Cartas.");
            return;
        }

        hand.Add(cartaComponent);

        cartaObj.transform.position = playerHand.position;
        cartaObj.transform.SetParent(playerHand);

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        SetLayerRecursively(cartaObj, playerLayer);

        Debug.Log("Uma carta foi adicionada � m�o do jogador. Agora o jogador tem " + hand.Count + " cartas na m�o.");
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
            Debug.LogError("GerenciadorIA n�o est� atribu�do no Player.");
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

        Debug.LogError("Nenhum slot dispon�vel para jogar a carta.");
        return -1;
    }
}
