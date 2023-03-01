using System;
using System.Collections.Generic;
using System.Linq;

namespace Ecubytes.Identity.Application.Api.Models
{
    public static class ModelConverter
    {
        public static List<ApplicationViewModelDTO> ToDTO(this IEnumerable<Application.Data.Models.App.Application> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }

        public static ApplicationViewModelDTO ToDTO(this Application.Data.Models.App.Application model)
        {
            return new ApplicationViewModelDTO()
            {
                ApplicationId = model.ApplicationId,
                HomePageUrl = model.HomePageUrl,
                Name = model.Name,
                PrivacyStatementUrl = model.PrivacyStatementUrl,
                StateId = (Data.Models.Constants.ApplicationStates)model.StateId,
                TermsOfServiceUrl = model.TermsOfServiceUrl
            };
        }

        public static List<RedirectUriViewModelDTO> ToDTO(this IEnumerable<Application.Data.Models.Auth.RedirectUri> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static RedirectUriViewModelDTO ToDTO(this Application.Data.Models.Auth.RedirectUri model)
        {
            return new RedirectUriViewModelDTO()
            {
                ApplicationId = model.ApplicationId,
                PlatformId = model.PlatformId,
                UriId = model.RedirectUriId,
                Uri = model.Uri,
                PlatformName = model.Platform?.Name
            };
        }

        public static List<ApplicationSecretViewModelDTO> ToDTO(this IEnumerable<Application.Data.Models.Auth.ApplicationSecret> model)
        {
            return model.Select(p => p.ToDTO()).ToList();
        }
        public static ApplicationSecretViewModelDTO ToDTO(this Application.Data.Models.Auth.ApplicationSecret model)
        {
            return new ApplicationSecretViewModelDTO()
            {
                ApplicationId = model.ApplicationId,
                CreationDate = model.CreationDate,
                Description = model.Description,
                ExpirationDate = model.ExpirationDate,                
                SecretId = model.ApplicationSecretId
            };
        }
    }
}
