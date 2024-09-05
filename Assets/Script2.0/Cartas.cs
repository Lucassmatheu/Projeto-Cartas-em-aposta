using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartas : MonoBehaviour
{
    [SerializeField] public naipes naipe;
    [SerializeField] public valoresNumeros valoresNumeros;
    [SerializeField] public SpriteRenderer image;
  


    
    private bool primeirarodada;

    private void Start()
    {
        primeirarodada = GameObject.Find("ComboDosJogos").GetComponent<Todas_Cartas>().primeiraRodada;
    }
    public Cartas(naipes Naipes, valoresNumeros valoresNumeros)
    {
        this.valoresNumeros = valoresNumeros;
        this.naipe = Naipes;

    }

    public class cartas
    {
        public string Naipes { get; set; }
        public string ValoresNumeros { get; set; }




        public cartas(string NaipesCartas, string ValoresNumerosCartas)
        {
            Naipes = NaipesCartas;
            ValoresNumeros = ValoresNumerosCartas;

        }
    }


    public class Deck
    {
        public List<cartas> Cards { get; set; }

        public Deck()
        {
            Cards = new List<cartas>();
            naipes[] todosNaipes = (naipes[])Enum.GetValues(typeof(naipes));
            valoresNumeros[] todosValores = (valoresNumeros[])Enum.GetValues(typeof(valoresNumeros));

            foreach (naipes naipe in todosNaipes)
            {


                foreach (valoresNumeros valor in todosValores)
                {
                    // Cria uma nova carta
                    cartas novaCarta = new cartas(naipe.ToString(), valor.ToString());

                    // Verifica se a carta já existe no baralho
                    bool cartaExiste = Cards.Exists(carta => carta.Naipes == novaCarta.Naipes && carta.ValoresNumeros == novaCarta.ValoresNumeros);

                    // Se a carta não existir no baralho, adiciona ao baralho
                    if (!cartaExiste)
                    {
                        Cards.Add(novaCarta);
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (primeirarodada)
        {
            image.sprite = Resources.Load<Sprite>("carta_virada");
           
        }
        else
        {
            image.sprite = Resources.Load<Sprite>("C_" + naipe + "_" + valoresNumeros);
          
        }
    }
}
