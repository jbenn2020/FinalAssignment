//This program gets a list of quotes from an API and inserts them into a DynamoDB database called 'FinalAssignment'.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;

using Amazon.Lambda.DynamoDBEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Lambda.APIGatewayEvents;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FinalInsertQuotes
{
    public class Function
    {
        private static AmazonDynamoDBClient dbClient = new AmazonDynamoDBClient();  //DynamoDB client
        private static string tableName = "FinalAssignment";    //Name of the table in DynamoDB
        public async Task<List<dynamic>> FunctionHandler(APIGatewayProxyRequest input, ILambdaContext context)
        {
            //Create an http client and expandoobject
            HttpClient client = new HttpClient();
            dynamic obj = new ExpandoObject();
            
            //Get the quotes from the api and put them into a result string
            HttpResponseMessage response = client.GetAsync("https://quote-garden.herokuapp.com/api/v3/quotes").Result;
            response.EnsureSuccessStatusCode();
            string result = response.Content.ReadAsStringAsync().Result;

            //Convert the result string to an ExpanoObject
            var converter = new Newtonsoft.Json.Converters.ExpandoObjectConverter();
            obj = JsonConvert.DeserializeObject<ExpandoObject>(result, converter);
            
            //Load the quotes tables
            Table quotes = Table.LoadTable(dbClient, tableName);

            //Get the data from the quotes object (we don't want the status codes or anything else, just the data).
            var json = JsonConvert.SerializeObject(obj.data);

            //Create a list of all of the data that's in the json object
            List<Quote> quoteList = JsonConvert.DeserializeObject<List<Quote>>(json);

            PutItemOperationConfig config = new PutItemOperationConfig();

            //Put the data into the table
            foreach (Quote q in quoteList)
            {
                Document res = await quotes.PutItemAsync(Document.FromJson(JsonConvert.SerializeObject(q)), config);
            }

            return obj.data;
        }
    }
}
