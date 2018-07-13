using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;

namespace OracleWebCenterExporter.Infrastructure
{
    public interface ICookieAwareWebClient
    {
        void Login(string loginPageAddress, NameValueCollection loginData);
        string DownloadString(string address);
    }

    // Hint to Visual Studio to create this simply as code in the solution explorer.
    [System.ComponentModel.DesignerCategory("Code")]
    public class CookieAwareWebClient : WebClient, ICookieAwareWebClient
    {
        public CookieContainer CookieContainer { get; private set; }

        public void Login(string loginPageAddress, NameValueCollection loginData)
        {
            var request = (HttpWebRequest)WebRequest.Create(loginPageAddress);

            try
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                var querystring = string.Join("&", loginData.AllKeys.Select(x => x + "=" + loginData[x]));

                var buffer = Encoding.ASCII.GetBytes(querystring);
                request.ContentLength = buffer.Length;
                var requestStream = request.GetRequestStream();
                requestStream.Write(buffer, 0, buffer.Length);
                requestStream.Close();
                request.AllowAutoRedirect = false;

                var container = request.CookieContainer = new CookieContainer();

                var response = request.GetResponse() as HttpWebResponse;

                foreach (Cookie cookie in response.Cookies)
                {
                    container.Add(cookie);
                }

                response.Close();

                CookieContainer = container;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public CookieAwareWebClient() : this(new CookieContainer()) { }

        public CookieAwareWebClient(CookieContainer container)
        {
            CookieContainer = container;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            var request = (HttpWebRequest)base.GetWebRequest(address);

            if (request == null)
            {
                throw new ApplicationException($"Could not get WebRequest for Uri: {address.AbsoluteUri}");
            }

            request.CookieContainer = CookieContainer;

            return request;
        }
    }
}
