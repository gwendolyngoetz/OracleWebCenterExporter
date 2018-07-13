using System;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace OracleWebCenterExporter.Infrastructure
{
    public class BindingGenerator
    {
        public static BasicHttpBinding CreateLargeSizeBasicHttpBinding()
        {
            const int maxValue = int.MaxValue;

            return new BasicHttpBinding
            {
                MaxBufferPoolSize = maxValue,
                MaxBufferSize = maxValue,
                MaxReceivedMessageSize = maxValue,
                ReceiveTimeout = new TimeSpan(0, 10, 0),
                ReaderQuotas =
                {
                    MaxDepth = 32,
                    MaxStringContentLength = maxValue,
                    MaxArrayLength = maxValue
                },
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport =
                    {
                        ClientCredentialType = HttpClientCredentialType.Basic,
                        ProxyCredentialType = HttpProxyCredentialType.None,
                        Realm = ""
                    },
                    Message =
                    {
                        ClientCredentialType = BasicHttpMessageCredentialType.UserName,
                        AlgorithmSuite = SecurityAlgorithmSuite.Default
                    }
                }
            };
        }
    }
}