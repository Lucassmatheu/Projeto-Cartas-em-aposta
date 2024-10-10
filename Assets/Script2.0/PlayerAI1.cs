using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerAI1 : PlayerAIBase
{
    private Player player;
   
    protected override void Start()
    {
        base.Start();
        playerLayerName = "Player2Card"; // Defina o nome da camada específica para PlayerAI1
        player = GetComponent<Player>();
        AdicionarCartaInicial();
        Debug.Log("PlayerAI1 Start - Carta inicial adicionada.");
    }

    protected void AdicionarCartaInicial()
    {
        GameObject cartaPrefab = ObterCartaAleatoria();
        AddCardToHand(cartaPrefab);
        Debug.Log("PlayerAI1 AdicionarCartaInicial - Carta inicial adicionada à mão.");
    }


    protected GameObject ObterCartaAleatoria()
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        var cartaAleatoria = todasCartas.CartaAleatoria();
        return cartaAleatoria.gameObject; // Retorna diretamente a instância da carta
    }

    protected void AddCardToHand(GameObject cartaObj)
    {
        if (hand.Count < 1) // Certifica-se de que apenas uma carta pode ser adicionada
        {
            hand.Add(cartaObj.GetComponent<Cartas>());
            cartaObj.transform.position = playerHand.position;
            cartaObj.transform.SetParent(playerHand);

            int playerLayer = LayerMask.NameToLayer(playerLayerName);
            SetLayerRecursively(cartaObj, playerLayer);

            Debug.Log("PlayerAI1 AddCardToHand - Uma carta foi adicionada à mão do jogador. Agora o jogador tem " + hand.Count + " cartas na mão.");
        }
        else
        {
            Debug.LogWarning("PlayerAI1 AddCardToHand - Tentativa de adicionar mais de uma carta à mão.");
        }
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
        Debug.Log("PlayerAI1 FazerDeclaracao - Decidindo declaração.");
        List<Cartas> cartasDosOutrosJogadores = ObterCartasDosOutrosJogadores();
        bool vaiGanhar = DecidirDeclaracao(cartasDosOutrosJogadores);
        FazerDeclaracaoBase(vaiGanhar);
    }

    protected override bool DecidirDeclaracao(List<Cartas> cartasDosOutrosJogadores)
    {
        var todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        int minhaForca = CalcularForcaDasMinhasCartas(); // Calcular a força das cartas na mão do jogador
        int forcaMaximaDosOutros = 0;

        // Verificar a força máxima das cartas dos outros jogadores
        foreach (var carta in cartasDosOutrosJogadores)
        {
            int forcaCarta = todasCartas.CalcularForcaDaCarta(carta);
            if (forcaCarta > forcaMaximaDosOutros)
            {
                forcaMaximaDosOutros = forcaCarta;
            }
        }

        // Decisão baseada na comparação entre a força do jogador IA e a maior força dos outros jogadores
        if (minhaForca > forcaMaximaDosOutros)
        {
            Debug.Log("PlayerAI1 DecidirDeclaracao - Declarando Ganhar.");
            return true; // Declarar "Ganhar"
        }
        else
        {
            Debug.Log("PlayerAI1 DecidirDeclaracao - Declarando Perder.");
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
        yield return new WaitForSeconds(1f); // Simulação de tempo de pensar
        if (hand.Count > 0)
        {
            Cartas cartaParaJogar = hand[0];
            GameObject cartaObj = cartaParaJogar.gameObject;
            int slotIndex = SelecionarSlotParaJogar();
            if (slotIndex >= 0)
            {
                mesaManager.ColocarCartaNaMesa(cartaObj, slotIndex, this);
                hand.Remove(cartaParaJogar);
                Debug.Log("PlayerAI1 JogarTurnoAutomatico - Jogou a carta: " + cartaParaJogar.valoresNumeros + " de " + cartaParaJogar.naipe);
            }
        }
    }
}