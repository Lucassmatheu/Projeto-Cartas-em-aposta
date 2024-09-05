using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MesaManager : MonoBehaviour
{
    public Transform[] slots; // Array de slots na mesa onde as cartas serão colocadas
    private Todas_Cartas todasCartas; // Referência à classe Todas_Cartas

    void Start()
    {
        todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();
        if (todasCartas == null)
        {
            Debug.LogError("Todas_Cartas não encontrado!");
        }
    }

    public void ColocarCartaNaMesa(GameObject cartaObj, int slotIndex, MonoBehaviour jogador)
    {
        // Colocar a carta no slot da mesa
        cartaObj.transform.SetParent(slots[slotIndex]);
        cartaObj.transform.position = slots[slotIndex].position;

        // Adicionar a carta na mesa na classe Todas_Cartas
        todasCartas.AdicionarCartaNaMesa(cartaObj.GetComponent<Cartas>(), jogador);
    }

    public int CalcularForcaDaCarta(Cartas carta)
    {
        return todasCartas.CalcularForcaDaCarta(carta);
    }
}
