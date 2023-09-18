using JuanMartin.RestApiClient.Entities;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using System.Net;

namespace JuanMartin.RestApiClient
{

    /// <summary>
    /// Class using api from https://rapidapi.com/rapidapi/api/movie-database-alternative/  
    /// var client = new RestClient("https://movie-database-alternative.p.rapidapi.com/?y=1998&r=json&i=tt4154796&type=movie");");
    /// var request = new RestRequest(Method.GET);
    /// request.AddHeader("X-RapidAPI-Key", "43e70c125emsh2f755ddda5a37ffp15fdd3jsn94d8227ec0ed");
    /// request.AddHeader("X-RapidAPI-Host", "movie-database-alternative.p.rapidapi.com");
    /// IRestResponse response = client.Execute(request);
    /// </summary>
    public class ImdbApi
    {
        public const string urlBase = "https://movie-database-alternative.p.rapidapi.com";

        public string GetImdbId(string title, string year="")
        {
            var url = string.Format("{0}/?s={1}&r=json", urlBase, title.Replace(" ", "%20"));
            if (year != "")
                url += string.Format("&y={0}", year);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Key", "43e70c125emsh2f755ddda5a37ffp15fdd3jsn94d8227ec0ed");
            request.AddHeader("X-RapidAPI-Host", "movie-database-alternative.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            if (response!=null && response.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                if(json!=null)
                {

                    if (json["Search"] is JArray search)
                    {
                        IJEnumerable<JProperty> properties = null;
                        JProperty selectedTitle = null;

                        foreach (var item in search.Children())
                        {
                            properties = item.Children<JProperty>();
                            selectedTitle = properties.FirstOrDefault(x => x.Name == "Title");

                            if (selectedTitle != null)
                                if (selectedTitle.Value.ToString().ToUpper() == title.ToUpper())
                                        break;
                        }
                        if (selectedTitle != null)
                        { 
                            return properties?.FirstOrDefault(x => x.Name == "imdbID").Value.ToString() ?? HttpStatusCode.NotFound.ToString();
                        }
                    }
                }
            }
            return HttpStatusCode.NotFound.ToString();
        }

        private Movie GetDetails(string id)
        {
            var m = new Movie();
            
            var url = string.Format("{0}/?i={1}&r=json&type=movie&plot=full", urlBase, id);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("X-RapidAPI-Key", "43e70c125emsh2f755ddda5a37ffp15fdd3jsn94d8227ec0ed");
            request.AddHeader("X-RapidAPI-Host", "movie-database-alternative.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            if (response != null && response.StatusCode == HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);

                if (json != null)
                {
                    foreach (var item in json.Children())
                    {
                        switch (item.Path)
                        {
                            case "imdbID":
                                {
                                    m.id = item.Last.ToString();
                                    break;
                                }
                            case "Title":
                                {
                                    m.title = item.Last.ToString();
                                    break;
                                }
                            case "Plot":
                                {
                                    m.plot = item.Last.ToString();
                                    break;
                                }
                            case "Year":
                                {
                                    m.year = item.Last.ToString();
                                    break;
                                }
                            case "Director":
                                {
                                    m.directors = item.Last.ToString().Split(",").ToList();
                                    break;
                                }
                            case "Runtime":
                                {
                                    var a = item.Last.ToString();
                                    var b = string.Empty;

                                    for (int i = 0; i < a.Length; i++)
                                    {
                                        if (Char.IsDigit(a[i]))
                                            b += a[i];
                                    }

                                    if (b.Length > 0)
                                        m.duration = int.Parse(b);

                                    break;
                                }
                            case "Genre":
                                {
                                    m.genres = item.Last.ToString().Split(",").ToList();
                                    break;
                                }
                            case "Released":
                                {
                                    string dateString = item.Last.ToString();
                                    DateTime dateValue;

                                    if (DateTime.TryParse(dateString, out dateValue))
                                        m.releaseDate = Convert.ToDateTime(dateString);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
            else
                return null;

            return m;
        }


    public Movie GetMovie(string title, string year="")
        {
            Movie m = null;

            var id = GetImdbId(title,year);

            if (id != HttpStatusCode.NotFound.ToString())
            {
                m = GetDetails(id);
            }

            return m;
        }
}
}