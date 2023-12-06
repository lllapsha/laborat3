using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laborat3
{
    public class Track 
    {
        private string title;
        private string author;

        public Track(string title, string author)
        {
            this.title = title;
            this.author = author;
        }
        public string Title { get { return title; } set { } }
        public string Author { get { return author; } set { } }
    }
}
