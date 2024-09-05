using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class PlayerAIBase : MonoBehaviour
{
    public List<Cartas> hand = new List<Cartas>();
    public Transform playerHand;
    public string playerLayerName;

    protected MesaManager mesaManager;
    protected DeclarationManager declarationManager;
    protected Todas_Cartas todasCartas; // Refer�ncia � classe Todas_Cartas para obter a manilha
    public TextMeshPro declarationText3D; // Refer�ncia ao TextMeshPro 3D para exibir a declara��o

    protected virtual void Start()
    {
        mesaManager = GameObject.Find("Mesa").GetComponent<MesaManager>();
        declarationManager = GameObject.FindObjectOfType<DeclarationManager>();
        todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();

        if (declarationManager == null)
        {
            Debug.LogError("DeclarationManager n�o encontrado na cena.");
        }

        if (todasCartas == null)
        {
            Debug.LogError("Todas_Cartas n�o encontrado na cena.");
        }

        if (declarationText3D == null)
        {
            Debug.LogError("TextMeshPro 3D para declara��es n�o encontrado.");
        }
    }

    public abstract void FazerDeclaracao();

    public virtual IEnumerator JogarTurnoAutomatico()
    {
        // Implementar l�gica para jogar automaticamente
        yield return null;
    }

    protected int SelecionarSlotParaJogar()
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

    public void MostrarCartasAoUsuario()
    {
        foreach (var carta in hand)
        {
            // L�gica para mostrar a carta ao usu�rio
            Debug.Log("Carta: " + carta.valoresNumeros + " de " + carta.naipe);
        }
    }

    protected List<Cartas> ObterCartasDosOutrosJogadores()
    {
        List<Cartas> cartasDosOutrosJogadores = new List<Cartas>();

        foreach (var player in FindObjectsOfType<PlayerAIBase>())
        {
            if (player != this)
            {
                cartasDosOutrosJogadores.AddRange(player.hand);
            }
        }

        return cartasDosOutrosJogadores;
    }

    protected abstract bool DecidirDeclaracao(List<Cartas> cartasDosOutrosJogadores);

    protected void FazerDeclaracaoBase(bool vaiGanhar)
    {
        Declarar(vaiGanhar);
        Debug.Log(gameObject.name + " declara: " + (vaiGanhar ? "Ganhar" : "Perder"));
    }

    public void Declarar(bool vaiGanhar)
    {
        // Implementa��o do m�todo Declarar
        string declaracao = vaiGanhar ? "Ganhar" : "Perder";
        if (declarationText3D != null)
        {
            declarationText3D.text = gameObject.name + " declara: " + declaracao;
        }
        Debug.Log(gameObject.name + " declara: " + declaracao);
    }
}
