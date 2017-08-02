using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchFighter
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] query = { "The University of Hong Kong" };

            foreach (string item in query)
            {
                Console.WriteLine("-----------------------------------");
                Console.WriteLine("Titulo: {0}", item);
            }
            Console.ReadKey();

        }

        //public static IList<Result> Search(string query)
        //{
        //    Console.WriteLine("Executing custom search for query: {0} ...", query);

        //    CseResource.ListRequest listRequest = Service.Cse.List(query);
        //    listRequest.Cx = cx;

        //    Search search = listRequest.Execute();
        //    return search.Items;
        //}
    }
}
