using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GerenciadorIA : MonoBehaviour
{
    public PlayerAI1 playerIA1; // Objeto PlayerAI1
    public PlayerAI1 playerIA2; // Objeto PlayerAI2
    public PlayerAI1 playerIA3; // Objeto PlayerAI3
    public Player jogadorHumano; // Objeto Player Humano

    public DeclarationManager declarationManager;

    private bool jogadorHumanoDeclarou = false;
    private int rodadaAtual = 1;

    // Fila para gerenciar a ordem dos turnos dos jogadores IA
    private Queue<PlayerAIBase> filaJogadores;
    private bool turnoEmProgresso;

    void Start()
    {
        // Inicializa a fila de jogadores IA
        filaJogadores = new Queue<PlayerAIBase>();
        filaJogadores.Enqueue(playerIA1); // Adiciona o PlayerAI1 na fila
        filaJogadores.Enqueue(playerIA2); // Adiciona o PlayerAI2 na fila
        filaJogadores.Enqueue(playerIA3); // Adiciona o PlayerAI3 na fila

        turnoEmProgresso = false;

        // Mostrar cartas dos jogadores automáticos na primeira rodada
        MostrarCartasPrimeiraRodada();

        // Iniciar a coroutine para gerenciar os turnos
        StartCoroutine(GerenciarTurnos());

        // Certifique-se de que declarationManager está atribuído
        if (declarationManager == null)
        {
            declarationManager = FindObjectOfType<DeclarationManager>();
            if (declarationManager == null)
            {
                Debug.LogError("DeclarationManager não encontrado!");
            }
        }
    }

    // Função para mostrar as cartas dos jogadores automáticos na primeira rodada
    private void MostrarCartasPrimeiraRodada()
    {
        foreach (var jogadorIA in filaJogadores)
        {
            jogadorIA.MostrarCartasAoUsuario(); // Chama o método de mostrar as cartas ao jogador humano
        }
    }

    // Coroutine para gerenciar os turnos
    private IEnumerator GerenciarTurnos()
    {
        while (true)
        {
            if (rodadaAtual == 1)
            {
                // Aguardar a declaração do jogador humano na primeira rodada
                yield return AguardarDeclaracaoJogadorHumano();
            }
            else
            {
                // Jogar turnos das IAs em rodadas subsequentes
                yield return JogarTurnosIA();
            }

            yield return new WaitForSeconds(1f); // Espera entre turnos
        }
    }

    // Coroutine para aguardar a declaração do jogador humano
    private IEnumerator AguardarDeclaracaoJogadorHumano()
    {
        // Aguardar até que o jogador humano faça sua declaração
        while (!jogadorHumanoDeclarou)
        {
            yield return null;
        }

        // Após o jogador humano declarar, as IAs fazem suas declarações
        playerIA1.FazerDeclaracao();
        playerIA2.FazerDeclaracao();
        playerIA3.FazerDeclaracao();

        // Esperar o jogador humano jogar
        yield return jogadorHumano.JogarTurno();

        // Iniciar o turno das IAs após o jogador humano
        yield return JogarTurnosIA();

        // Passar para a próxima rodada
        rodadaAtual++;
    }

    // Coroutine para gerenciar os turnos dos jogadores IA
    private IEnumerator JogarTurnosIA()
    {
        foreach (var jogadorIA in filaJogadores)
        {
            turnoEmProgresso = true;
            // Jogador IA faz sua jogada automaticamente
            yield return jogadorIA.JogarTurnoAutomatico();
            turnoEmProgresso = false;
        }

        // Reorganizar a fila de jogadores para que o próximo jogador seja o primeiro a jogar na rodada seguinte
        filaJogadores.Enqueue(filaJogadores.Dequeue());
    }

    // Função para capturar a declaração do jogador humano (se vai ganhar ou perder)
    public void DeclaracaoJogadorHumano(bool vaiGanhar)
    {
        jogadorHumanoDeclarou = true; // Marca que o jogador humano já declarou

        // Atualiza o texto de declaração do jogador humano
        if (declarationManager != null && declarationManager.declaracaoJogadorHumano != null)
        {
            declarationManager.declaracaoJogadorHumano.text = "Jogador Humano declara: " + (vaiGanhar ? "Ganhar" : "Perder");
        }
        else
        {
            Debug.LogError("DeclarationManager ou declaracaoJogadorHumano não está atribuído no GerenciadorIA.");
        }
    }
}
