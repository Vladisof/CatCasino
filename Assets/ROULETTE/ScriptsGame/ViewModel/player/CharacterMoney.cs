using UniRx;
using UnityEngine;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.player
{
    [CreateAssetMenu(fileName = "New Character Money", menuName = "Scriptable/Character Money")]
    public class CharacterMoney : ScriptableObject
    {
        public IntReactiveProperty characterBet = new IntReactiveProperty();
        public IntReactiveProperty characterMoney = new IntReactiveProperty();
        public IntReactiveProperty currentPayment = new IntReactiveProperty();

        private void AddCash(int cashWinner)
        {
            int aux = characterMoney.Value;
            characterMoney.Value += cashWinner;
        }

        private void SubCash(int cashLost)
        {
            if(cashLost < 0) 
            {
                cashLost = cashLost * -1;
            }

            characterMoney.Value -= cashLost;

            if (characterMoney.Value < 0)
            {
                characterMoney.Value = 0;
            }
        }

        private void AddBet(int betSum)
        {
            int aux = characterBet.Value - betSum;
            characterBet.Value  += betSum;
        }

        private void SubstractBet(int betRest)
        {
            int aux = characterBet.Value ;
            characterBet.Value  -= betRest;
        }
        
        public bool CheckBetValue(int valueFiche)
        {
            bool aux = true;
            if (valueFiche <= characterMoney.Value  && valueFiche != 0)
            {
                aux = true;
                SubCash(valueFiche);
                AddBet(valueFiche);
            }
            else
            {
                aux = false;
            }
            return aux;
        }
        public void DeleteChip(int valueFiche)
        {

            SubstractBet(valueFiche);
            AddCash(valueFiche);
        }
        public void PaymentSystem(int payment)
        {      
            characterBet.Value = 0;
            if(payment > 0)
                AddCash(payment);
        }
    }
}
