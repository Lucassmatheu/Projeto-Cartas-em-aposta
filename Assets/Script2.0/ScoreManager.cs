using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private Dictionary<string, int> scores = new Dictionary<string, int>();

    // Inicializa os jogadores com suas pontuações em 0
    public void InicializarJogadores(List<string> nomesJogadores)
    {
        foreach (var nome in nomesJogadores)
        {
            scores[nome] = 0; // Inicializa a pontuação de cada jogador em 0
        }
    }

    // Calcula a pontuação de acordo com as declarações dos jogadores
    public void CalcularPontuacao(bool jogadorHumanoGanhou, string declaracaoHumano,
                                   bool ia1Ganhou, string declaracaoIA1,
                                   bool ia2Ganhou, string declaracaoIA2,
                                   bool ia3Ganhou, string declaracaoIA3)
    {
        // Lógica para calcular a pontuação do jogador humano
        if (jogadorHumanoGanhou)
        {
            scores["JogadorHumano"] += (declaracaoHumano == "Ganhar") ? 1 : -1;
        }
        else
        {
            scores["JogadorHumano"] += (declaracaoHumano == "Ganhar") ? -1 : 0;
        }

        // Lógica para calcular a pontuação da IA1
        if (ia1Ganhou)
        {
            scores["IA1"] += (declaracaoIA1 == "Ganhar") ? 1 : -1;
        }
        else
        {
            scores["IA1"] += (declaracaoIA1 == "Ganhar") ? -1 : 0;
        }

        // Lógica para calcular a pontuação da IA2
        if (ia2Ganhou)
        {
            scores["IA2"] += (declaracaoIA2 == "Ganhar") ? 1 : -1;
        }
        else
        {
            scores["IA2"] += (declaracaoIA2 == "Ganhar") ? -1 : 0;
        }

        // Lógica para calcular a pontuação da IA3
        if (ia3Ganhou)
        {
            scores["IA3"] += (declaracaoIA3 == "Ganhar") ? 1 : -1;
        }
        else
        {
            scores["IA3"] += (declaracaoIA3 == "Ganhar") ? -1 : 0;
        }

        // Exibir pontuações para depuração
        foreach (var entry in scores)
        {
            Debug.Log(entry.Key + " pontuação: " + entry.Value);
        }
    }

    // Retorna a pontuação de um jogador específico
    public int GetPontuacao(string nomeJogador)
    {
        if (scores.TryGetValue(nomeJogador, out int pontuacao))
        {
            return pontuacao; // Retorna a pontuação do jogador se ele existir no dicionário
        }
        return 0; // Retorna 0 se o jogador não estiver no dicionário
    }
}
