using System;
using System.Collections.Generic;
using System.Text;

namespace FinalInsertQuotes
{
    [Serializable]
    public class Quote
    {
        public string _id { get; set; }
        public string quoteText { get; set; }
        public string quoteAuthor { get; set; }
        public string quoteGenre { get; set; }
        //public string __v { get; set; }

        public Quote(string i, string qt, string qa, string qg)
        {
            _id = i;
            quoteText = qt;
            quoteAuthor = qa;
            quoteGenre = qg;
        }

        public Quote()
        {

        }
    }
}
