using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Scripts.ROULETTE.ScriptsGame.Components.hud;
using Scripts.ROULETTE.ScriptsGame.ViewModel.player;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Editor.Tests.Components.hud
{
    public class GameMoneyDisplayShould
    {
        private GameObject _gameObject;
        private GameObject _gameObjectBet;
        private GameObject _gameObjectMoney;

        private TextMeshProUGUI _textFieldBet;
        private TextMeshProUGUI _textFieldMoney;

        [SetUp]
        public void SetUp()
        {
            _gameObject = new GameObject();
            _gameObjectBet = new GameObject();
            _gameObjectMoney = new GameObject();
            
            _textFieldBet = _gameObjectBet.AddComponent<TextMeshProUGUI>();
            _textFieldMoney = _gameObjectMoney.AddComponent<TextMeshProUGUI>();
            _textFieldBet.text = "";
        }

        [Test]
        public void show_player_money()
        {
            var component = _gameObject.AddComponent<GameMoneyDisplay>();
            component.betLabel = _textFieldBet;
            component.moneyLabel = _textFieldMoney;
            component.characterTable = ScriptableObject.CreateInstance<CharacterTable>();
            component.characterTable.characterMoney = ScriptableObject.CreateInstance<CharacterMoney>();
            component.Start();

            component.characterTable.characterMoney.characterBet.Value = 20;  
            component.characterTable.characterMoney.characterMoney.Value = 100;  

            Assert.AreEqual("20", _textFieldBet.text);
            Assert.AreEqual("100", _textFieldMoney.text);
        }
    }
}
