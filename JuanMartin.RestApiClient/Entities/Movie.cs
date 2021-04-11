using System;
using System.Collections.Generic;

namespace JuanMartin.RestApiClient.Entities
{
    public class Movie
    {
        public string id;
        public string title;
        public List<string> directors;
        public int duration;
        public string year;
        public DateTime releaseDate;
        public List<string> genres;
        public string plot;
    }
}
