using System;
using System.ServiceModel;
using GetFileSvcModel = OracleWebCenterExporter.GetFileSvc;
using DocInfoSvcModel = OracleWebCenterExporter.DocInfoSvc;

namespace OracleWebCenterExporter.Infrastructure
{
    internal class SoapClientFactory
    {
        public static SoapClientFactory Instance { get; } = new SoapClientFactory();

        private SoapClientFactory() { }

        public GetFileSvcModel.GetFileSoapClient CreateGetFileSoapClient(string endpoint, string username, string password)
        {
            var binding = BindingGenerator.CreateLargeSizeBasicHttpBinding();
            var endpointAddress = new EndpointAddress(endpoint);

            var client = new GetFileSvcModel.GetFileSoapClient(binding, endpointAddress);

            if (client.ClientCredentials == null)
            {
                throw new ArgumentNullException($"Unable to create soap client to uri: {endpoint}");
            }

            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;

            return client;
        }

        public DocInfoSvcModel.DocInfoSoapClient CreateDocInfoSoapClient(string endpoint, string username, string password)
        {
            var binding = BindingGenerator.CreateLargeSizeBasicHttpBinding();
            var endpointAddress = new EndpointAddress(endpoint);

            var client = new DocInfoSvcModel.DocInfoSoapClient(binding, endpointAddress);

            if (client.ClientCredentials == null)
            {
                throw new ArgumentNullException($"Unable to create soap client to uri: {endpoint}");
            }

            client.ClientCredentials.UserName.UserName = username;
            client.ClientCredentials.UserName.Password = password;

            return client;
        }
    }
}