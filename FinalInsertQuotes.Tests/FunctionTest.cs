using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;

using FinalInsertQuotes;

namespace FinalInsertQuotes.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void TestToUpperFunction()
        {
            Quote quote = new Quote("test", "I am a test quote", "Jeffrey Benn", "testGenere");

            Assert.Equal("Jeffrey Benn", quote.quoteAuthor);
        }
    }
}
