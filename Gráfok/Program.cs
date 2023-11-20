using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gráfok
{
    class Program
    {
        static void Main()
        {
            // Tesztelés
            Graph<Person> socialNetwork = new Graph<Person>();

            // Példányosítások
            Person stew = new Person { Name = "Stew" };
            Person marge = new Person { Name = "Marge" };
            Person joseph = new Person { Name = "Joseph" };
            Person gerald = new Person { Name = "Gerald" };
            Person zack = new Person { Name = "Zack" };
            Person peter = new Person { Name = "Peter" };
            Person janet = new Person { Name = "Janet" };

            // Élek hozzáadása a gráfhoz
            socialNetwork.AddEdge(stew, joseph);
            socialNetwork.AddEdge(stew, marge);
            socialNetwork.AddEdge(marge, joseph);
            socialNetwork.AddEdge(joseph, stew);
            socialNetwork.AddEdge(joseph, marge);
            socialNetwork.AddEdge(joseph, gerald);
            socialNetwork.AddEdge(joseph, zack);
            socialNetwork.AddEdge(gerald, joseph);
            socialNetwork.AddEdge(gerald, zack);
            socialNetwork.AddEdge(zack, joseph);
            socialNetwork.AddEdge(zack, gerald);
            socialNetwork.AddEdge(zack, peter);
            socialNetwork.AddEdge(peter, zack);
            socialNetwork.AddEdge(peter, janet);
            socialNetwork.AddEdge(janet, peter);

            // Szélességi bejárás
            Console.WriteLine("BFS:");
            socialNetwork.BFS(zack, Console.WriteLine);

            // Mélységi bejárás
            Console.WriteLine("\nDFS:");
            socialNetwork.DFS(zack, Console.WriteLine);

            // Feliratkozás az eseményre és élek kiírása
            socialNetwork.EdgeAdded += (sender, e) =>
            {
                Console.WriteLine($"\nEdge added: {e.NodeA} -> {e.NodeB}");
            };

            // Ismeretség fokának kiírása
            int jg = socialNetwork.DegreeOfConnection(janet, gerald);
            Console.WriteLine($"\nDegree of connection between Janet and Gerald: {jg}");

            // Esemény kiváltása
            socialNetwork.AddEdge(janet, gerald);

            jg = socialNetwork.DegreeOfConnection(janet, gerald);
            Console.WriteLine($"\nDegree of connection between Janet and Gerald: {jg}");

            Console.ReadLine();
        }
    }
}