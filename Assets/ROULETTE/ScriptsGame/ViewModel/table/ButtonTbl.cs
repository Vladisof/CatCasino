using UnityEngine;
using UnityEngine.Serialization;

namespace Scripts.ROULETTE.ScriptsGame.ViewModel.table
{    
    [CreateAssetMenu(fileName = "New Button Table", menuName = "Scriptable/Button Table")]
    public class ButtonTbl : ScriptableObject
    {
        public string buttonName;
        public int[] buttonValue;
        public KeyButton buttonKey;
        [FormerlySerializedAs("isPleno")]
        public bool isPlano;

        public int currentChipsOnTop;
        public Vector2 currentSpritePivot;
        public Vector2 currentOffset;  

        public Vector2 GetOffset()
        {
            Vector2 offset = currentOffset;

            Vector2 v = new Vector2(0.01f,0.038f);
            currentOffset = currentOffset + v;
            
            return offset;
        }
        public Vector2 SubstrateCurrentOffset()
        {
            Vector2 v = new Vector2(0.01f,0.038f);
            currentOffset = currentOffset - v;
            return currentOffset;
        }
    }

    public enum KeyButton
    {
        NumberPlano,
        NumberMiddle,
        Dozen,
        Column,
        EvenOdd,
        Eighteenth,
        BlackRed
    }
}
