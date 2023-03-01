using System;
using Ecubytes.Data;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Ecubytes.AspNetCore.Mvc.TagHelpers
{
    [HtmlTargetElement(Template.ComponentsName.MessageBlock)]
    public class MessageBlockTagHelper : TagHelper
    {
        private ILogger<MessageBlockTagHelper> logger;
        private IHtmlHelper htmlHelper;
        public MessageBlockTagHelper(ILogger<MessageBlockTagHelper> logger,
            IHtmlHelper htmlHelper)
        {
            this.logger = logger;
            this.htmlHelper = htmlHelper;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("asp-messages")]
        public MessageColletion Messages { get; set; }
        [HtmlAttributeName("asp-message")]
        public Message Message { get; set; }
        [HtmlAttributeName("asp-text")]
        public string Text { get; set; }
        [HtmlAttributeName("asp-type")]
        public MessageType? MessageType { get; set; }
        [HtmlAttributeName("asp-allow-close")]
        public bool AllowClose { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.Add(
                Template.ComponentsName.ComponentAttributeName,
                Template.ComponentsName.MessageBlock);

            MessageColletion _messages = null;

            if (Messages != null && Messages.Any())
            {
                _messages = Messages;
            }
            else if (Message != null)
            {
                _messages = new MessageColletion()
                {
                    Message
                };
            }
            else if (!string.IsNullOrWhiteSpace(Text))
            {
                _messages = new MessageColletion()
                {
                    new Data.Message(Text, MessageType ?? Data.MessageType.Information)
                };
            }
            else
            {
                (htmlHelper as IViewContextAware).Contextualize(ViewContext);
                var msgs = htmlHelper.ViewBag._EcubytesMessages as MessageCollection;
                
                if(msgs!=null && msgs.Any())
                {
                    _messages = new MessageColletion();
                    _messages.AddRange(msgs);
                }
            }

            if (_messages != null && _messages.Any())
            {
                StringBuilder content = new StringBuilder();
                foreach (var msg in _messages)
                {
                    content.AppendLine(BuilderMessage(msg));
                }

                output.Content.SetHtmlContent(content.ToString());
            }
        }

        private string GetMessageClass(MessageType? type)
        {
            if (!type.HasValue)
                return Template.MessageBlockTemplate.PrimaryClass;

            switch (type.Value)
            {
                case Data.MessageType.Information:
                    return Template.MessageBlockTemplate.InfoClass;
                case Data.MessageType.Error:
                    return Template.MessageBlockTemplate.DangerClass;
                case Data.MessageType.Success:
                    return Template.MessageBlockTemplate.SuccessClass;
                case Data.MessageType.Warning:
                    return Template.MessageBlockTemplate.WarningClass;
                default:
                    return Template.MessageBlockTemplate.PrimaryClass;
            }
        }


        private string BuilderMessage(Message message)
        {
            TagBuilder tag = new TagBuilder("div");
            tag.AddCssClass(GetMessageClass(message.Type));

            tag.InnerHtml.AppendHtml(message.Text);
            tag.MergeAttributes(Template.MessageBlockTemplate.Attributes);

            if(this.AllowClose)
            {
                tag.AddCssClass(Template.MessageBlockTemplate.DismissibleClass);

                TagBuilder buttonClose = new TagBuilder("button");                
                buttonClose.AddCssClass(Template.MessageBlockTemplate.ButtonCloseClass);
                buttonClose.MergeAttributes(Template.MessageBlockTemplate.ButtonCloseAttributes);

                tag.InnerHtml.AppendHtml(buttonClose.RenderToString());
            }

            //tag.TagRenderMode = TagRenderMode.EndTag;
            return tag.RenderToString();   
        }
    }
}
