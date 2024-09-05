using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class JogadorBase : MonoBehaviour
{
    public List<Cartas> hand = new List<Cartas>();
    [SerializeField] protected Transform playerHand;
    [SerializeField] protected string playerLayerName = "Player1Card";
    [SerializeField] protected TextMeshProUGUI resultadoText; // Referência ao TextMeshProUGUI para mostrar o resultado

    protected MesaManager mesaManager;
    protected Cartas cartaJogada;
    protected static bool primeiraMao = true;

    protected virtual void Start()
    {
        mesaManager = GameObject.Find("Mesa").GetComponent<MesaManager>();
        if (primeiraMao)
        {
            AdicionarCartaInicial();
            primeiraMao = false;
        }
        else
        {
            AdicionarCartas(2); // Adiciona duas cartas nas rodadas subsequentes
        }
    }

    protected void AdicionarCartaInicial()
    {
        GameObject cartaPrefab = ObterCartaAleatoria();
        AddCardToHand(cartaPrefab);
    }

    protected void AdicionarCartas(int quantidade)
    {
        for (int i = 0; i < quantidade; i++)
        {
            GameObject cartaPrefab = ObterCartaAleatoria();
            AddCardToHand(cartaPrefab);
        }
    }

    protected GameObject ObterCartaAleatoria()
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        var cartaAleatoria = todasCartas.CartaAleatoria();
        var cartasComMesmoNaipe = GameObject.FindGameObjectsWithTag(cartaAleatoria.naipe.ToString());
        foreach (var go in cartasComMesmoNaipe)
        {
            if (go.GetComponent<Cartas>().valoresNumeros == cartaAleatoria.valoresNumeros)
            {
                return go;
            }
        }
        return null;
    }

    protected void AddCardToHand(GameObject cartaObj)
    {
        hand.Add(cartaObj.GetComponent<Cartas>());
        cartaObj.transform.position = playerHand.position;
        cartaObj.transform.SetParent(playerHand);

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        SetLayerRecursively(cartaObj, playerLayer);

        Debug.Log("Uma carta foi adicionada à mão do jogador. Agora o jogador tem " + hand.Count + " cartas na mão.");
    }

    protected void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;
        obj.layer = newLayer;
        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public abstract IEnumerator JogarTurno();

    public Cartas ObterCartaJogada()
    {
        return cartaJogada;
    }

    public bool PossuiCarta(Cartas carta)
    {
        return hand.Contains(carta);
    }

    protected bool VerificarSeGanhou()
    {
        // Implementar a lógica para determinar se o jogador ganhou
        return true; // Substitua isso com a lógica real
    }

    protected void AtualizarTextoResultado(bool ganhou)
    {
        if (ganhou)
        {
            resultadoText.text = "Ganhou na primeira mão!";
        }
        else
        {
            resultadoText.text = "Perdeu na primeira mão.";
        }
    }

    protected virtual int SelecionarSlotParaJogar()
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
