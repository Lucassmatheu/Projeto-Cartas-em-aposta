using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameController : MonoBehaviour
{
    public GameObject ControladorDeBotao;
    private Todas_Cartas todasCartas;

    void Start()
    {
        // Encontrar o componente Todas_Cartas na cena
        todasCartas = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>();

        // Ativar o ControladorDeBotao na primeira rodada
        ControladorDeBotao.SetActive(true);
    }

    void Update()
    {
        // Desativar o ControladorDeBotao após a primeira rodada
        if (todasCartas.primeiraRodadaTerminou)
        {
            ControladorDeBotao.SetActive(false);
        }
    }
}
