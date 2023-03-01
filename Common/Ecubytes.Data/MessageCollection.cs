using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Data
{
    [Serializable]
    public class MessageColletion : List<Message>
    {        
        public bool HasError()
        {
            return this.Any(p => p.Type == MessageType.Error);
        }
        public bool IsValid
        {
            get
            {
                return !HasError();
            }            
        }
        public void AddMessage(string text)
        {
            Add(new Message(text));
        }
        public void AddMessage(string text, MessageType type)
        {
            Add(new Message(text, type));
        }
        public void AddMessage(string text, MessageType type, string title)
        {
            Add(new Message(text, type, title));
        }
        public void AddMessage(string text, MessageType type, int code)
        {
            Add(new Message(text, type, code));
        }
        public void AddMessage(string text, MessageType type, string title, int code)
        {
            Add(new Message(text, type, title, code));
        }
    }
}