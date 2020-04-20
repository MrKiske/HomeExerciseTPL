using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace RestAPI
{
    internal class Program
    {
        //
        public static void Main(string[] args)
        {
            var url = string.Empty;
            var postId = string.Empty;


            Console.WriteLine($"Please, type postId...");
            postId = (string)(Console.ReadLine().Trim());

            using (HttpClient client = new HttpClient())
            {
                if (string.IsNullOrEmpty(postId))
                {
                    url = "https://jsonplaceholder.typicode.com/posts";
                    client.BaseAddress = new Uri(url);
                }
                else
                {
                    url = $"https://jsonplaceholder.typicode.com/comments?postId={postId}";
                    client.BaseAddress = new Uri(url);
                }

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    Task<string> task = ReadStringAsync(response);
                    Console.Write($"Code answer: {response.StatusCode}; Details: {task.Result}");
                }
                else
                {
                    Console.Write("postId not found!");
                }
            }

        }

        private static Task<string> ReadStringAsync(HttpResponseMessage response)
        {
            return Task.Factory.StartNew(() =>
            {
                return response.Content.ReadAsStringAsync().Result;
            });

        }
    }
}
