using System;
using System.Collections.Generic;
using Ecubytes.Data;
using Microsoft.Extensions.Logging;

namespace Ecubytes.AspNetCore.Mvc.Controllers
{
    public partial class ControllerBase
    {
        #region MessageServices

        protected MessageCollection Messages
        {
            get
            {
                return messageCollection ?? (messageCollection = new MessageCollection());
            }
        }
        protected bool IsValidState
        {
            get
            {
                return this.Messages.IsValid;
            }
        }

        protected void AddMessages(ModelResult modelResult)
        {
            AddMessages(modelResult.Messages);
        }
        protected void AddMessages(MessageCollection messageCollection)
        {
            foreach(var m in messageCollection)            
                this.AddMessage(m);
            //this.Messages.SetMessages(messageCollection);
        }
        protected ControllerBase AddDefaultSuccessMessage()
        {
            this.AddSuccessMessage("Message.DefaultSuccess", "Message.TitleDefaultSuccess");
            return this;
        }
        protected ControllerBase AddDefaultErrorMessage()
        {
            this.AddErrorMessage("Message.DefaultError", "Message.TitleDefaultError");
            return this;
        }
        protected ControllerBase AddDefaultErrorMessage(Exception ex)
        {
            this.AddErrorMessage(ex, "Message.DefaultError", "Message.TitleDefaultError");
            return this;
        }
        protected ControllerBase AddErrorMessage(string text)
        {
            this.AddMessage(text, MessageType.Error);
            return this;
        }
        protected ControllerBase AddErrorMessage(string text, string title)
        {
            this.AddMessage(text, MessageType.Error, title);
            return this;
        }
        protected ControllerBase AddErrorMessage(Exception ex)
        {
            return this.AddMessage(new Message(ex.Message, MessageType.Error, null, null, ex));
        }
        protected ControllerBase AddErrorMessage(Exception ex, string text)
        {
            return this.AddMessage(new Message(text, MessageType.Error, null, null, ex));
        }
        protected ControllerBase AddErrorMessage(Exception ex, string text, string title)
        {
            this.AddMessage(new Message(text, MessageType.Error, title, null, ex));
            return this;
        }
        protected ControllerBase AddErrorMessage(Exception ex, string text, int code)
        {
            return this.AddMessage(new Message(text, MessageType.Error, code));
        }
        protected ControllerBase AddErrorMessage(Exception ex, string text, string title, int code)
        {
            this.AddMessage(new Message(text, MessageType.Error, title, code, ex));
            return this;
        }
        protected ControllerBase AddErrorMessage(string text, int code)
        {
            this.AddMessage(new Message(text, MessageType.Error, code));
            return this;
        }
        protected ControllerBase AddInformationMessage(string text)
        {
            this.AddMessage(text, MessageType.Information);
            return this;
        }
        protected ControllerBase AddInformationMessage(string text, string title)
        {
            this.AddMessage(text, MessageType.Information, title);
            return this;
        }
        protected ControllerBase AddInformationMessage(string text, int code)
        {
            this.AddMessage(text, MessageType.Information, code);
            return this;
        }
        protected ControllerBase AddSuccessMessage(string text)
        {
            this.AddMessage(text, MessageType.Success);
            return this;
        }
        protected ControllerBase AddSuccessMessage(string text, string title)
        {
            this.AddMessage(text, MessageType.Success, title);
            return this;
        }
        protected ControllerBase AddSuccessMessage(string text, int code)
        {
            this.AddMessage(text, MessageType.Success, code);
            return this;
        }
        protected ControllerBase AddWarningMessage(string text)
        {
            this.AddMessage(text, MessageType.Warning);
            return this;
        }
        protected ControllerBase AddWarningMessage(string text, string title)
        {
            this.AddMessage(text, MessageType.Warning, title);
            return this;
        }
        protected ControllerBase AddWarningMessage(string text, int code)
        {
            this.AddMessage(text, MessageType.Warning, code);
            return this;
        }
        protected ControllerBase AddWarningMessage(Exception ex, string text)
        {
            this.AddMessage(new Message(text, MessageType.Warning, null, null, ex));
            return this;
        }
        protected ControllerBase AddWarningMessage(Exception ex, string text, string title)
        {
            this.AddMessage(new Message(text, MessageType.Warning, title, null, ex));
            return this;
        }
        protected ControllerBase AddWarningMessage(Exception ex, string text, int code)
        {
            this.AddMessage(new Message(text, MessageType.Warning, code));
            return this;
        }
        protected ControllerBase AddWarningMessage(Exception ex, string text, string title, int code)
        {
            this.AddMessage(new Message(text, MessageType.Warning, title, code, ex));
            return this;
        }
        protected ControllerBase AddMessage(string message)
        {
            this.AddMessage(new Message(message));
            return this;
        }
        protected ControllerBase AddMessage(string message, MessageType type)
        {
            this.AddMessage(new Message(message, type));
            return this;
        }
        protected ControllerBase AddMessage(string message, string title)
        {
            this.AddMessage(new Message(message, MessageType.Information, title));
            return this;
        }
        protected ControllerBase AddMessage(string message, MessageType type, string title)
        {
            this.AddMessage(new Message(message, type, title));
            return this;
        }
        protected ControllerBase AddMessage(string message, MessageType type, int code)
        {
            this.AddMessage(new Message(message, type, code));
            return this;
        }
        protected ControllerBase AddMessage(string message, MessageType type, string title, int code)
        {
            this.AddMessage(new Message(message, type, title, code));
            return this;
        }
        protected ControllerBase AddMessageRequiredField(string fieldName)
        {
            return AddMessageRequiredField(fieldName, null);
        }
        protected ControllerBase AddMessageRequiredField(string fieldName, string internalFieldName)
        {
            string message = GetLocalizableString("Message.RequiredField");
            fieldName = this.GetLocalizableString(fieldName);

            if (!string.IsNullOrEmpty(internalFieldName))
                internalFieldName = $"({internalFieldName})";

            this.AddErrorMessage(string.Format(message, fieldName, internalFieldName));
            return this;
        }
        protected ControllerBase AddMessageInvalidFieldValue(string fieldName)
        {
            return AddMessageInvalidFieldValue(fieldName, null);
        }
        protected ControllerBase AddMessageInvalidFieldValue(string fieldName, string internalFieldName)
        {
            string message = GetLocalizableString("The value specified for {0}{1} is invalid");
            fieldName = this.GetLocalizableString(fieldName);

            if (!string.IsNullOrEmpty(internalFieldName))
                internalFieldName = $"({internalFieldName})";

            this.AddErrorMessage(string.Format(message, fieldName, internalFieldName));
            return this;
        }
        protected ControllerBase AddMessage(Message message)
        {
            if (!string.IsNullOrWhiteSpace(message.Text))
                message.Text = GetLocalizableString(message.Text);
            if (!string.IsNullOrWhiteSpace(message.Title))
                message.Title = GetLocalizableString(message.Title);

            this.Messages.Add(message);

            switch (message.Type)
            {
                case MessageType.Information:
                    Logger.LogInformation(exception: message.Exception, message: message.Text);
                    break;
                case MessageType.Warning:
                    Logger.LogWarning(exception: message.Exception, message.Text);
                    break;
                case MessageType.Success:
                    Logger.LogInformation(exception: message.Exception, message.Text);
                    break;
                case MessageType.Error:
                    Logger.LogError(exception: message.Exception, message.Text);
                    break;
            }

            return this;
        }

        protected void AddMessages(IEnumerable<Message> messages)
        {
            foreach (var item in messages)
            {
                this.AddMessage(item);
            }
        }
        protected void AddMessages(IModelMessageCollection model)
        {
            this.AddMessages(model.Messages);
        }
        protected void ClearMessages()
        {
            this.Messages.Clear();
        }

        #endregion

    }
}
