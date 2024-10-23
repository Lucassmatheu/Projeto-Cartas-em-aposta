using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    public GameObject placarPanel; // Referência ao painel de placar no Canvas
    public TextMeshProUGUI pontuacoesText; // Referência para o texto das pontuações
    public List<PlayerData> players; // Lista de jogadores com nomes e pontuações
    public GameObject placarButton; // Botão para alternar o placar

    private ScoreManager scoreManager; // Referência ao ScoreManager
    private bool isPlacarVisible = false; // Variável que controla a visibilidade do painel de placar

    void Start()
    {
        // Inicializa o placar invisível
        placarPanel.SetActive(isPlacarVisible);
        scoreManager = new ScoreManager();
        InicializarJogadores(); // Inicializa os jogadores
        AtualizarTextoPontuacoes(); // Atualiza as pontuações na primeira vez
    }

    // Inicializa os jogadores no ScoreManager
    private void InicializarJogadores()
    {
        // Limpa a lista de jogadores
        players = new List<PlayerData>
        {
            new PlayerData { nome = "JogadorHumano", pontuacao = 0, declaracao = "Ganhar" }, // Jogador humano
            new PlayerData { nome = "IA1", pontuacao = 0, declaracao = "Ganhar" }, // IA1
            new PlayerData { nome = "IA2", pontuacao = 0, declaracao = "Perder" }, // IA2
            new PlayerData { nome = "IA3", pontuacao = 0, declaracao = "Ganhar" }  // IA3
        };

        // Inicializa pontuações no ScoreManager
        List<string> nomesJogadores = new List<string>();
        foreach (var player in players)
        {
            nomesJogadores.Add(player.nome); // Adiciona os nomes dos jogadores à lista
        }
        scoreManager.InicializarJogadores(nomesJogadores); // Inicializa pontuações no ScoreManager
    }

    // Função que alterna a visibilidade do placar ao clicar no botão
    public void AlternarPlacar()
    {
        isPlacarVisible = !isPlacarVisible;
        placarPanel.SetActive(isPlacarVisible);
        placarButton.SetActive(true); // Mantém o botão ativo

        Debug.Log($"Placar visível: {isPlacarVisible}");
        Debug.Log($"Botão ativo: {placarButton.activeSelf}");

        if (isPlacarVisible)
        {
            AtualizarTextoPontuacoes();
            CentralizarPlacar();
        }
    }

    // Atualiza o texto das pontuações no placar
    private void AtualizarTextoPontuacoes()
    {
        string pontuacaoTexto = "Pontuações:\n"; // Cabeçalho do placar

        // Adiciona o nome e a pontuação de cada jogador na lista
        foreach (var player in players)
        {
            pontuacaoTexto += $"{player.nome}: {scoreManager.GetPontuacao(player.nome)}\n"; // Usa o ScoreManager para obter a pontuação
        }

        pontuacoesText.text = pontuacaoTexto; // Define o texto no campo de pontuações
        pontuacoesText.fontSize = 30; // Aumenta o tamanho da fonte para 30
    }

    // Função para centralizar o placar na tela
    private void CentralizarPlacar()
    {
        RectTransform placarRect = placarPanel.GetComponent<RectTransform>();
        placarRect.anchoredPosition = Vector2.zero; // Define a posição no centro da tela
    }

    // Método para calcular e atualizar a pontuação de todos os jogadores após uma rodada
    public void AtualizarPontuacoes(bool jogadorHumanoGanhou, string declaracaoHumano,
                                     bool ia1Ganhou, string declaracaoIA1,
                                     bool ia2Ganhou, string declaracaoIA2,
                                     bool ia3Ganhou, string declaracaoIA3)
    {
        scoreManager.CalcularPontuacao(jogadorHumanoGanhou, declaracaoHumano,
                                         ia1Ganhou, declaracaoIA1,
                                         ia2Ganhou, declaracaoIA2,
                                         ia3Ganhou, declaracaoIA3);

        // Atualiza as pontuações na lista de players
        foreach (var player in players)
        {
            player.pontuacao = scoreManager.GetPontuacao(player.nome); // Atualiza a pontuação do jogador
        }

        AtualizarTextoPontuacoes(); // Atualiza o texto do placar
        MostrarResultado(jogadorHumanoGanhou, declaracaoHumano, ia1Ganhou, declaracaoIA1, ia2Ganhou, declaracaoIA2, ia3Ganhou, declaracaoIA3);
    }

    // Método para mostrar o resultado da rodada
    public void MostrarResultado(bool jogadorHumanoGanhou, string declaracaoHumano,
                                 bool ia1Ganhou, string declaracaoIA1,
                                 bool ia2Ganhou, string declaracaoIA2,
                                 bool ia3Ganhou, string declaracaoIA3)
    {
        string resultado = "Resultado da Rodada:\n";

        if (jogadorHumanoGanhou)
        {
            resultado += "Jogador Humano: Ganhou\n";
        }
        else
        {
            resultado += "Jogador Humano: Perdeu\n";
        }

        resultado += $"IA1: {(ia1Ganhou ? "Ganhou" : "Perdeu")}\n";
        resultado += $"IA2: {(ia2Ganhou ? "Ganhou" : "Perdeu")}\n";
        resultado += $"IA3: {(ia3Ganhou ? "Ganhou" : "Perdeu")}\n";

        Debug.Log(resultado); // Exibe o resultado no console
        // Aqui você pode adicionar lógica para exibir o resultado em um TextMeshPro na UI, por exemplo
        // resultadoTextMeshPro.text = resultado; // Atualiza um TextMeshPro para mostrar o resultado na tela
    }
}

// Classe para armazenar os dados de cada jogador
[System.Serializable]
public class PlayerData
{
    public string nome; // Nome do jogador
    public int pontuacao; // Pontuação do jogador
    public string declaracao; // A declaração do jogador ("Ganhar" ou "Perder")
}
