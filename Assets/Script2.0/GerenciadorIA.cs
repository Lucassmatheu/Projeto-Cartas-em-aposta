using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GerenciadorIA : MonoBehaviour
{
    public PlayerAI1 playerIA1;
    public PlayerAI2 playerIA2;
    public PlayerAI3 playerIA3;
    public Player jogadorHumano;

    public DeclarationManager declarationManager;

    private bool jogadorHumanoDeclarou = false;
    private int rodadaAtual = 1;

    private Queue<PlayerAIBase> filaJogadores;
    private bool turnoEmProgresso;

    void Start()
    {
        filaJogadores = new Queue<PlayerAIBase>();
        filaJogadores.Enqueue(playerIA1);
        filaJogadores.Enqueue(playerIA2);
        filaJogadores.Enqueue(playerIA3);

        turnoEmProgresso = false;

        // Mostrar cartas dos jogadores autom�ticos na primeira rodada
        MostrarCartasPrimeiraRodada();

        // Iniciar a coroutine para gerenciar os turnos
        StartCoroutine(GerenciarTurnos());

        // Certifique-se de que declarationManager est� atribu�do
        if (declarationManager == null)
        {
            declarationManager = FindObjectOfType<DeclarationManager>();
            if (declarationManager == null)
            {
                Debug.LogError("DeclarationManager n�o encontrado!");
            }
        }
    }

    private void MostrarCartasPrimeiraRodada()
    {
        foreach (var jogadorIA in filaJogadores)
        {
            jogadorIA.MostrarCartasAoUsuario();
        }
    }

    private IEnumerator GerenciarTurnos()
    {
        while (true)
        {
            if (rodadaAtual == 1)
            {
                yield return AguardarDeclaracaoJogadorHumano();
            }
            else
            {
                yield return JogarTurnosIA();
            }

            yield return new WaitForSeconds(1f); // Tempo entre turnos
        }
    }

    private IEnumerator AguardarDeclaracaoJogadorHumano()
    {
        while (!jogadorHumanoDeclarou)
        {
            // Aguardar at� que o jogador humano fa�a uma declara��o
            yield return null;
        }

        // Acionar as declara��es das IAs ap�s o jogador humano declarar
        playerIA1.FazerDeclaracao();
        playerIA2.FazerDeclaracao();
        playerIA3.FazerDeclaracao();

        // Esperar o jogador humano jogar
        yield return jogadorHumano.JogarTurno();

        // Iniciar o turno das IAs
        yield return JogarTurnosIA();

        // Passar para a pr�xima rodada
        rodadaAtual++;
    }

    private IEnumerator JogarTurnosIA()
    {
        foreach (var jogadorIA in filaJogadores)
        {
            turnoEmProgresso = true;
            yield return jogadorIA.JogarTurnoAutomatico();
            turnoEmProgresso = false;
        }

        // Reorganizar a fila de jogadores para a pr�xima rodada
        filaJogadores.Enqueue(filaJogadores.Dequeue());
    }

    public void DeclaracaoJogadorHumano(bool vaiGanhar)
    {
        jogadorHumanoDeclarou = true;
        if (declarationManager != null && declarationManager.declaracaoJogadorHumano != null)
        {
            declarationManager.declaracaoJogadorHumano.text = "Jogador Humano declara: " + (vaiGanhar ? "Ganhar" : "Perder");
        }
        else
        {
            Debug.LogError("DeclarationManager ou declaracaoJogadorHumano n�o est� atribu�do no GerenciadorIA.");
        }
    }
}
