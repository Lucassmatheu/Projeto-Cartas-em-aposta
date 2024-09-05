using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Importa o namespace do TextMeshPro

public class PlayerAI2 : PlayerAIBase
{
    protected override void Start()
    {
        base.Start();
        playerLayerName = "Player3Card"; // Defina o nome da camada espec�fica para PlayerAI2
        AdicionarCartaInicial();
        Debug.Log("PlayerAI2 Start - Carta inicial adicionada.");
    }

    protected void AdicionarCartaInicial()
    {
        GameObject cartaPrefab = ObterCartaAleatoria();
        AddCardToHand(cartaPrefab);
        Debug.Log("PlayerAI2 AdicionarCartaInicial - Carta inicial adicionada � m�o.");
    }

    protected GameObject ObterCartaAleatoria()
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        var cartaAleatoria = todasCartas.CartaAleatoria();
        var cartas = GameObject.FindGameObjectsWithTag(cartaAleatoria.naipe.ToString());
        foreach (var go in cartas)
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

        Debug.Log("PlayerAI2 AddCardToHand - Uma carta foi adicionada � m�o do jogador. Agora o jogador tem " + hand.Count + " cartas na m�o.");
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

    public override void FazerDeclaracao()
    {
        Debug.Log("PlayerAI2 FazerDeclaracao - Decidindo declara��o.");
        List<Cartas> cartasDosOutrosJogadores = ObterCartasDosOutrosJogadores();
        bool vaiGanhar = DecidirDeclaracao(cartasDosOutrosJogadores);

        // Exibe a declara��o no TextMeshPro 3D
        if (declarationText3D != null)
        {
            declarationText3D.text = vaiGanhar ? "Vou ganhar!" : "Vou perder!";
        }
        else
        {
            Debug.LogError("Declaration Text 3D n�o est� atribu�do no PlayerAI2.");
        }

        FazerDeclaracaoBase(vaiGanhar);
    }

    protected override bool DecidirDeclaracao(List<Cartas> cartasDosOutrosJogadores)
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        int minhaForca = CalcularForcaDasMinhasCartas(); // Calcular a for�a das cartas na m�o do jogador
        int forcaMaximaDosOutros = 0;

        // Verificar a for�a m�xima das cartas dos outros jogadores
        foreach (var carta in cartasDosOutrosJogadores)
        {
            int forcaCarta = todasCartas.CalcularForcaDaCarta(carta);
            if (forcaCarta > forcaMaximaDosOutros)
            {
                forcaMaximaDosOutros = forcaCarta;
            }
        }

        // Decis�o baseada na compara��o entre a for�a do jogador IA e a maior for�a dos outros jogadores
        if (minhaForca > forcaMaximaDosOutros)
        {
            Debug.Log("PlayerAI2 DecidirDeclaracao - Declarando Ganhar.");
            return true; // Declarar "Ganhar"
        }
        else
        {
            Debug.Log("PlayerAI2 DecidirDeclaracao - Declarando Perder.");
            return false; // Declarar "Perder"
        }
    }

    private int CalcularForcaDasMinhasCartas()
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        int totalForca = 0;

        foreach (var carta in hand)
        {
            totalForca += todasCartas.CalcularForcaDaCarta(carta);
        }

        return totalForca;
    }

    public override IEnumerator JogarTurnoAutomatico()
    {
        yield return new WaitForSeconds(1f); // Simula��o de tempo de pensar
        if (hand.Count > 0)
        {
            Cartas cartaParaJogar = hand[0];
            GameObject cartaObj = cartaParaJogar.gameObject;
            int slotIndex = SelecionarSlotParaJogar();
            if (slotIndex >= 0)
            {
                mesaManager.ColocarCartaNaMesa(cartaObj, slotIndex, this);
                hand.Remove(cartaParaJogar);
                Debug.Log("PlayerAI2 JogarTurnoAutomatico - Jogou a carta: " + cartaParaJogar.valoresNumeros + " de " + cartaParaJogar.naipe);
            }
        }
    }
}
