using System;
using System.Linq;
using Ecubytes.Identity.Application.Data.Models.Constants;

namespace Ecubytes.Identity.Application.Api.Models
{
     public abstract class ApplicationBaseDTO
    {
        public string Name { get; set; }        
        public string HomePageUrl { get; set; }
        public string PrivacyStatementUrl { get; set; }
        public string TermsOfServiceUrl { get; set; }
    }

    public class ApplicationInsertDTO : ApplicationBaseDTO
    {
    }

    public class ApplicationUpdateDTO : ApplicationBaseDTO
    {
        public Guid ApplicationId { get; set; }
        public ApplicationStates StateId { get; set; }
    }

    public class ApplicationInsertResultDTO
    {
        public Guid ApplicationId { get; set; }
    }

    public class ApplicationViewModelDTO : ApplicationBaseDTO
    {
        public Guid ApplicationId { get; set; }
        public ApplicationStates StateId { get; set; }
    }
}
