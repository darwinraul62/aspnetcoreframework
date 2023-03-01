using System;

namespace Ecubytes.Data
{    
    public class ModelResult : IModelMessageCollection
    {
        public bool IsValid
        {
            get
            {
                return !HasErrors();
            }
        }  
        public bool HasErrors()
        {
            return this.Messages.HasError();
        }

        public void AddMessages(ModelResult modelResult)
        {
            this.Messages.AddRange(modelResult.Messages);
        }

        private MessageCollection messages { get; set; }
        public MessageCollection Messages
        {
            get
            {
                if (messages == null)
                    messages = new MessageCollection();

                return messages;
            }
            set
            {
                messages = value;
            }
        }
       
        // public Message Message
        // {
        //     get
        //     {
        //         if (Messages.Any())                  
        //             return this.Messages.First();
        //         return null;
        //     }
        // }        
    }

    public class ModelResult<T> : ModelResult
    {
        public T Data { get; set; }
    }
}
