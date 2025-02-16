using System.Collections.Generic;
using TinyTrails.Characters;
using TinyTrails.DTO;
using TinyTrails.Managers;
using TinyTrails.Render;
using TinyTrails.Types;
using UnityEngine;

namespace TinyTrails.Actions
{
    public class DefenseAction : MonoBehaviour
    {
        Player _player;
        int _defense;

        public void Init()
        {
            _player = GameManager.Instance.Player;

            GameManager.Instance.EventManager.Publisher<int>(EventChannelType.OnUIDefenseChange, _player.Stats.Defense);

            _defense = _player.Stats.Defense;
            GameManager.Instance.EventManager.Subscriber(EventChannelType.OnTurnPlayerStart, OnStartTurnPlayer);
        }

        public void Action()
        {
            _defense += 2;

            UIRender.DefensePushLabelUIRender(GameManager.Instance.Player.transform.position);

            GameManager.Instance.EventManager.Publisher(EventChannelType.OnUIDefenseChange, _defense);
        }

        #region Events
        void OnStartTurnPlayer()
        {
            _defense = _player.Stats.Defense;
        }
        #endregion
    }
}