using UnityEngine;
using TMPro;

public class PlacarController : MonoBehaviour
{
    public TextMeshProUGUI placarText; // Campo de texto para o placar

    void Start()
    {
        AtualizarPlacar();
    }

    void AtualizarPlacar()
    {
        // Exemplo de pontuação ou nomes de jogadores
        string placar = "Jogador 1: 10\nJogador 2: 15\nJogador 3: 8";
        placarText.text = placar; // Define o texto no campo de placar
    }
}
