using JuanMartin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace JuanMartin.RestApiClient
{
    internal class Program
    {
        private static void Main()
        {
            var key = Console.ReadKey();

            switch (key.KeyChar)
            {
                case 'E':
                    {
                        try
                        {
                            var euler = new EulerApi();
                            var exam = File.ReadAllText(@"..\..\..\exam-sample.json");
                            var r = euler.GetAnswers(JsonConvert.DeserializeObject<Exam>(exam));
                            var json = JsonConvert.SerializeObject(r, Formatting.Indented);
                            Console.WriteLine(json);
                            break;
                        }
                        catch (SerializationException e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                        catch (JsonException e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                    }
                case 'e':
                    {
                        try
                        {
                            var euler = new EulerApi();
                            var r = euler.GetAnswer("2", new Dictionary<string, string>() { { "intnumber", "4000000" } });
                            var json = JsonConvert.SerializeObject(r, Formatting.Indented);
                            Console.WriteLine(json);
                            break;
                        }
                        catch (SerializationException e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                        catch (JsonException e)
                        {
                            Console.WriteLine(e.Message);
                            break;
                        }
                    }
                case 'm':
                    {
                        var imdb = new ImdbApi();
                        var movie = imdb.GetMovie("Avengers Endgame");
                        var json = JsonConvert.SerializeObject(movie, Formatting.Indented);
                        Console.WriteLine(json);
                        break;
                    }
                default:
                    break;

            }

        }
    }
}