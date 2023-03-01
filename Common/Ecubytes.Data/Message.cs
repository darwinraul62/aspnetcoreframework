using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Ecubytes.Data
{
    [DataContract, Serializable]
    public class Message
    {
        public Message()
        {
        }

        public Message(string text)
            : this(text, MessageType.Information, null, null, null)
        {
        }

        public Message(string text, MessageType type)
            : this(text, type, null, null, null)
        {
        }

        public Message(string text, string title)
            : this(text, MessageType.Information, title)
        {
        }

        public Message(string text, MessageType type, string title)
        : this(text, type, title, null, null)
        {
        }
        public Message(string text, MessageType type, int code)
        : this(text, type, null, code, null)
        {
        }
        public Message(string text, MessageType type, string title, int? code)
            : this(text, type, title, code, null)
        {
        }
        public Message(Exception ex)
            : this(ex.Message, MessageType.Error, null, null, ex)
        {
        }
        public Message(string text, MessageType type, string title, int? code, Exception ex)
        {
            this.Text = text;
            this.Type = type;
            this.Title = title;
            this.Code = code;
            this.Exception = ex;
        }

        [DataMember]
        public int? Code { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        // [EnumDataType(typeof(MessageType))]
        // [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }

        [DataMember]
        public string Title { get; set; }

        [JsonIgnore, IgnoreDataMember]
        public Exception Exception { get; set; }
    }
}
