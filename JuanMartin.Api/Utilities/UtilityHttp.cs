using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using JuanMartin.Models;
using System.Linq;

namespace JuanMartin.Api.Utilities
{
    public class UtilityHttp
    {
        public static HttpResponseMessage ResponseType(HttpRequestMessage request, HttpStatusCode code)
        {
            var resp = request.CreateResponse(code);

            return resp;
        }
        public static HttpResponseMessage ResponseNoProblem(HttpRequestMessage request)
        {
            return ResponseType(request, HttpStatusCode.NoContent);
        }

        public static HttpResponseMessage ResponseBadRequest(HttpRequestMessage request)
        {
            return ResponseType(request, HttpStatusCode.BadRequest);
        }

        public static HttpResponseMessage ResponseBadRequest(HttpRequestMessage request, string errorMessge)
        {
            var resp = ResponseType(request, HttpStatusCode.BadRequest);
            resp.Content = new StringContent(errorMessge);

            return resp;
        }

        public static HttpResponseMessage ResponseOk(HttpRequestMessage request, IEnumerable<Result> results)
        {
            object source;

            if (results.Count() == 1)
                source = results.First();
            else
                source = results;

            var json = JsonConvert.SerializeObject(source, Formatting.Indented);

            var resp = request.CreateResponse(HttpStatusCode.OK);
            resp.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            return resp;
        }
    }
}