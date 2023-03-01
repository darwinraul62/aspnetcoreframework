using System;
using System.Collections.Generic;
using System.Linq;
using Ecubytes.Configuration;
using Ecubytes.Data;
using Ecubytes.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Ecubytes.AspNetCore.Mvc.Controllers
{
    public partial class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        private MessageCollection messageCollection;
        private ILogger logger;
        private List<IStringLocalizer> localizers;
        private GlobalOptions globalOptions;
        public ControllerBase()
        {
        }

        public bool IsApiController
        {
            get
            {
                return this.GetType().GetCustomAttributes(typeof(ApiControllerAttribute), true).Any();
            }
        }     

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!this.IsApiController)
            {
                //Use TempData for shared Messages on Redirect Actions
                if (Ecubytes.DependencyInjection.ServiceActivator.CheckService(typeof(ITempDataDictionaryFactory)))
                {
                    string messagesTemp = TempData["_EcubytesMessages"] as string;
                    TempData.Remove("_EcubytesMessages");
                    if (messagesTemp != null)
                        ViewBag._EcubytesMessages = JsonUtility.Deserialize<MessageCollection>(messagesTemp);
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (!this.IsApiController)
            {
                //Shared Messages to View and on Redirect Ations
                if (this.Messages.Any())
                {
                    if (Ecubytes.DependencyInjection.ServiceActivator.CheckService(typeof(ITempDataDictionaryFactory)))
                    {
                        TempData["_EcubytesMessages"] = JsonUtility.Serialize(this.Messages);
                        ViewBag._EcubytesMessages = this.Messages;
                    }
                }
                // else
                // {
                //     if (Ecubytes.DependencyInjection.ServiceActivator.CheckService(typeof(ITempDataDictionaryFactory)))
                //     {
                //         TempData.Remove("_EcubytesMessages");
                //         ViewBag._EcubytesMessages = null;
                //     }
                // }
            }
        }

        protected string JsonSerialize(object model)
        {
            return System.Text.Json.JsonSerializer.Serialize(model);
        }

        #region GlobalOptions

        public GlobalOptions GlobalOptions
        {
            get
            {
                if (globalOptions == null)
                {
                    InitGlobalOptions();
                }
                return globalOptions;
            }
        }

        private void InitGlobalOptions()
        {
            using (var services = Ecubytes.DependencyInjection.ServiceActivator.GetScope())
            {
                var options = (IOptions<GlobalOptions>)services.ServiceProvider.GetService(typeof(IOptions<GlobalOptions>));
                if (options != null)
                {
                    globalOptions = options.Value;
                }
                else
                    globalOptions = new GlobalOptions();
            }
        }

        #endregion

        #region Localizer

        protected void AddLocalizer(IStringLocalizer localizer)
        {
            this.Localizers.Insert((Localizers.Count - 1), localizer);
        }

        private void InitLocalizers()
        {
            localizers = new List<IStringLocalizer>();

            using (var services = Ecubytes.DependencyInjection.ServiceActivator.GetScope())
            {
                IStringLocalizerFactory stringLocalizerFactory = (IStringLocalizerFactory)services.ServiceProvider.GetService(typeof(IStringLocalizerFactory));
                if (stringLocalizerFactory != null)
                {
                    localizers.Add(stringLocalizerFactory.Create(this.GetType()));
                    localizers.Add(stringLocalizerFactory.Create(typeof(ControllerBase)));
                }
            }
        }

        private List<IStringLocalizer> Localizers
        {
            get
            {
                if (localizers == null)
                {
                    InitLocalizers();
                }

                return localizers;
            }
        }

        protected string GetLocalizableString(string text)
        {
            foreach (var localizer in Localizers)
            {
                var localizableEntry = localizer[text];
                if (localizableEntry.ResourceNotFound)
                    continue;
                else
                {
                    text = localizableEntry;
                    break;
                }
            }
            return text;
        }

        protected string GetLocalizableString(string textStringFormat, params string[] args)
        {
            return string.Format(GetLocalizableString(textStringFormat), args);
        }


        #endregion

        #region Logger

        public ILogger Logger
        {
            get
            {
                if (logger == null)
                {
                    ILoggerFactory factory = (ILoggerFactory)this.HttpContext.RequestServices.GetService(typeof(ILoggerFactory));
                    logger = factory.CreateLogger(this.GetType());
                }

                return logger;
            }
        }

        #endregion


    }
}
