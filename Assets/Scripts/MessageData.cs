using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon
{
    public enum MessageType
    {
        None,
        WildBattle,
        TrainerBattle,
        PokemonSent,
        Win,
        Lose,
        Faint,
        SuperEffective,
        NotEffective,
        InEffective,
        Withdrawn,
        MoveUsed,
        Critical,
        StatIncreased,
        StatDecreased
    }
    
    [Serializable] public class Message
    {
        public MessageType _type;
        public string _message;
    }

    [CreateAssetMenu(fileName = "MessageData", menuName = "Pokemon/Message Data")]
    public class MessageData: ScriptableObject
    {
        public List<Message> _messages;

        public string GetMessage(MessageType messageType)
        {
            if (messageType == MessageType.None) return "";

            foreach (var message in _messages)
            {
                if (messageType == message._type) return message._message;
            }

            return null;
        }
    }
}