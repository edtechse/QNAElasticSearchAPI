using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnAElasticSearchService.Helper
{
    public class JsonConfiguration
    {
        private readonly IConfiguration JsonConfigurations;

        public JsonConfiguration(IConfiguration configuration)
        {
            JsonConfigurations = configuration;
        }

        public string GetUrl()
        {
            var myKeyValue = JsonConfigurations["APIUrlEndpoint"].ToString(); ;
            return myKeyValue;
        }

        public (string, string) GetBasicAuthDetails()
        {
            var basicAuthId = JsonConfigurations["BasicAuthId"].ToString();
            var basicAuthPwd = JsonConfigurations["BasicAuthPwd"].ToString();
            return (basicAuthId, basicAuthPwd);
        }
    }
}
