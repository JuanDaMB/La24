using UnityEngine;

namespace NewThings.Scripts.GameAndUI
{
    [CreateAssetMenu(menuName =  "Coin")]
    public class CoinData: ScriptableObject
    {
        public Sprite sprite;
        public Sprite spriteMarcado;
        private int _value;

        public int Value
        {
            get => _value;
            set => _value = value;
        }
    }
}
