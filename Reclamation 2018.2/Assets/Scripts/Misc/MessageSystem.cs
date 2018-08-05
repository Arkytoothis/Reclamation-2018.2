using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;
using TMPro;

namespace Reclamation.Gui
{
    public class MessageSystem : Singleton<MessageSystem>
    {
        [SerializeField] GameObject messagePrefab;
        [SerializeField] List<GameMessage> messages;
        [SerializeField] Transform messagesParent;

        static int MaxNumberMessages = 10;

        public void Initialize()
        {
            messages = new List<GameMessage>();
        }

        void Update()
        {
        }

        public void AddMessage(string text)
        {
            if (messages.Count >= MaxNumberMessages)
            {
                Destroy(messages[0].label.gameObject.transform.parent.gameObject);
                messages.Remove(messages[0]);
            }

            GameMessage message = new GameMessage();
            message.text = text;

            GameObject go = Instantiate(messagePrefab, messagesParent);
            message.label = go.GetComponentInChildren<TMP_Text>();
            message.label.text = message.text;

            messages.Add(message);
        }
    }
}