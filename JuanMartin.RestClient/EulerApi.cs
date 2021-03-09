using JuanMartin.Models;
using RestSharp;
using RestSharp.Serialization.Json;
using System.Collections.Generic;

namespace JuanMartin.RestApiClient
{
    class EulerApi
    {
        public const string baseUrl = "http://localhost/JuanMartin.Api/api/problems";

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

            request.AddJsonBody(arguments);
            request.AddHeader("accept", "application/json");

            IRestResponse response = client.Execute(request);

            var deserial = new JsonDeserializer();

            return deserial.Deserialize<IEnumerable<Result>>(response);
        }

    }
}

