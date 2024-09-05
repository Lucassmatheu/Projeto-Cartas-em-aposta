//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class AIManager : MonoBehaviour
//{
//    public Player jogadorHumano;
//    public PlayerAI1 jogadorIA1;
//    public PlayerAI2 jogadorIA2;
//    public PlayerAI3 jogadorIA3;
//    public DeclarationManager managerDeDeclaracoes; // Campo atualizado para o novo DeclarationManager
//    private bool jogadorHumanoDeclarou = false;

//    private Queue<PlayerAIBase> filaJogadores;
//    private bool turnoEmProgresso;

//    private int rodadaAtual = 1;

//    void Start()
//    {
//        filaJogadores = new Queue<PlayerAIBase>();
//        filaJogadores.Enqueue(jogadorIA1);
//        filaJogadores.Enqueue(jogadorIA2);
//        filaJogadores.Enqueue(jogadorIA3);

//        turnoEmProgresso = false;

//        // Iniciar a coroutine para gerenciar os turnos
//        StartCoroutine(GerenciarTurnos());
//    }

//    private IEnumerator GerenciarTurnos()
//    {
//        while (true)
//        {
//            if (rodadaAtual == 1)
//            {
//                jogadorHumano.FazerDeclaracao(); // Solicitar a declara��o do jogador humano na primeira rodada
//                yield return AguardarDeclaracaoJogadorHumano();
//            }
//            else
//            {
//                yield return JogarTurnosIA();
//            }

//            yield return new WaitForSeconds(1f); // Tempo entre turnos
//        }
//    }

//    private IEnumerator AguardarDeclaracaoJogadorHumano()
//    {
//        while (!jogadorHumanoDeclarou)
//        {
//            // Aguardar at� que o jogador humano fa�a uma declara��o
//            yield return null;
//        }

//        // Mostrar cartas das IAs ao usu�rio na primeira rodada
//        jogadorIA1.MostrarCartasAoUsuario();
//        jogadorIA2.MostrarCartasAoUsuario();
//        jogadorIA3.MostrarCartasAoUsuario();

//        // Acionar as declara��es das IAs ap�s o jogador humano declarar
//        jogadorIA1.FazerDeclaracao();
//        jogadorIA2.FazerDeclaracao();
//        jogadorIA3.FazerDeclaracao();

//        // Esperar o jogador humano jogar
//        yield return jogadorHumano.JogarTurno();

//        // Iniciar o turno das IAs
//        yield return JogarTurnosIA();

//        // Passar para a pr�xima rodada
//        rodadaAtual++;
//    }

//    private IEnumerator JogarTurnosIA()
//    {
//        foreach (var jogadorIA in filaJogadores)
//        {
//            turnoEmProgresso = true;
//            yield return jogadorIA.JogarTurnoAutomatico();
//            turnoEmProgresso = false;
//        }

//        // Reorganizar a fila de jogadores para a pr�xima rodada
//        filaJogadores.Enqueue(filaJogadores.Dequeue());
//    }

//    public void DeclaracaoJogadorHumano(bool vaiGanhar)
//    {
//        jogadorHumanoDeclarou = true;
//        if (managerDeDeclaracoes != null && managerDeDeclaracoes.declaracaoJogadorHumano != null)
//        {
//            managerDeDeclaracoes.declaracaoJogadorHumano.text = "Jogador Humano declara: " + (vaiGanhar ? "Ganhar" : "Perder");
//        }
//        else
//        {
//            Debug.LogError("DeclarationManager ou declaracaoJogadorHumano n�o est� atribu�do no GerenciadorIA.");
//        }
//    }
//}
