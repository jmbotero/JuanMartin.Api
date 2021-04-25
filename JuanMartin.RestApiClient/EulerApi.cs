using JuanMartin.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Xml;

namespace JuanMartin.RestApiClient
{
    class EulerApi
    {
        public string baseUrl;

        public EulerApi()
        {   
            baseUrl = GetBaseWebApiUrl();
        }
        public Result GetAnswer(string id, Dictionary<string, string> arguments)
        {
            var client = new RestClient(string.Format("{0}/{1}", baseUrl, id));
            var request = new RestRequest(Method.GET);

            request.AddQueryParameter("id", id);
            foreach (var arg in arguments)
                request.AddQueryParameter(arg.Key, arg.Value);

            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            var deserial = new JsonDeserializer();

            return deserial.Deserialize<Result>(response);
        }

        public IEnumerable<Result> GetAnswers(Exam arguments)
        {
            var client = new RestClient(baseUrl);
            var request = new RestRequest(Method.POST);
            IEnumerable<Result> r = null;

            request.AddJsonBody(arguments);
            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            if (response == null)
                Console.WriteLine("JuanMartin.Api is not responding!");
            else
            {
                var deserial = new JsonDeserializer();

                r = deserial.Deserialize<IEnumerable<Result>>(response);
            }
            return r;
        }

        private string GetBaseWebApiUrl()
        {
            string fullPathName = @"..\..\..\JuanMartin.Api\JuanMartin.Api.csproj";
            XmlDocument doc = new XmlDocument();

            XmlNamespaceManager namespaces = new XmlNamespaceManager(doc.NameTable);
            namespaces.AddNamespace("ns", "http://schemas.microsoft.com/developer/msbuild/2003");
            doc.Load(fullPathName);

            XmlNode urlNodeItem = doc.SelectSingleNode("/ns:Project/ns:ProjectExtensions/ns:VisualStudio/ns:FlavorProperties/ns:WebProjectProperties/ns:IISUrl", namespaces);

            string url = urlNodeItem.InnerText + "api/problems";

            return url;
        }
    }
}

