//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using UnityEngine;

//public class PlayerAutomatico : JogadorBase
//{
//    [SerializeField] private int[] allowedSlots; // Slots permitidos para este jogador automático

//    public override IEnumerator JogarTurno()
//    {
//        yield return new WaitForSeconds(1f); // Simula tempo de processamento

//        if (hand.Count > 0)
//        {
//            Cartas cartaParaJogar = hand[0];
//            GameObject cartaObj = cartaParaJogar.gameObject;
//            int slotIndex = SelecionarSlotParaJogar();
//            if (slotIndex >= 0)
//            {
//                mesaManager.ColocarCartaNaMesa(cartaObj, slotIndex);
//                hand.Remove(cartaParaJogar);
//                cartaJogada = cartaParaJogar;

//                bool ganhou = VerificarSeGanhou();
//                AtualizarTextoResultado(ganhou);
//            }
//            else
//            {
//                Debug.LogError("Nenhum slot disponível ou permitido para jogar a carta.");
//            }
//        }
//    }

//    protected override int SelecionarSlotParaJogar()
//    {
//        foreach (int slotIndex in allowedSlots)
//        {
//            if (slotIndex >= 0 && slotIndex < mesaManager.slots.Length && mesaManager.slots[slotIndex].childCount == 0)
//            {
//                return slotIndex;
//            }
//        }

//        Debug.LogError("Nenhum slot disponível ou permitido para jogar a carta.");
//        return -1;
//    }
//}
