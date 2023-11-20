using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gráfok
{
    // Osztály a gráf éleinél bekövetkező eseményekhez
    public class GraphEventArgs<T> : EventArgs
    {
        public T NodeA { get; }
        public T NodeB { get; }

        public GraphEventArgs(T nodeA, T nodeB)
        {
            NodeA = nodeA;
            NodeB = nodeB;
        }
    }

    // Delegált az eseményhez
    public delegate void GraphEventHandler<T>(object source, GraphEventArgs<T> e);

    // Gráf osztály generikus típussal
    public class Graph<T>
    {
        private Dictionary<T, List<T>> adjacencyList;

        // Esemény az él felvételéhez
        public event GraphEventHandler<T> EdgeAdded;

        public Graph()
        {
            adjacencyList = new Dictionary<T, List<T>>();
        }

        public void AddNode(T node)
        {
            if (!adjacencyList.ContainsKey(node))
            {
                adjacencyList[node] = new List<T>();
            }
        }

        public void AddEdge(T from, T to)
        {
            AddNode(from);
            AddNode(to);

            adjacencyList[from].Add(to);
            adjacencyList[to].Add(from);

            // Esemény kiváltása
            OnEdgeAdded(from, to);
        }

        public bool HasEdge(T from, T to)
        {
            return adjacencyList.ContainsKey(from) && adjacencyList[from].Contains(to);
        }

        public List<T> Neighbors(T node)
        {
            return adjacencyList.ContainsKey(node) ? adjacencyList[node] : new List<T>();
        }

        // Szélességi bejárás implementáció
        public void BFS(T startNode, Action<T> processNode)
        {
            HashSet<T> visited = new HashSet<T>();
            Queue<T> queue = new Queue<T>();

            queue.Enqueue(startNode);
            visited.Add(startNode);

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();
                processNode(current);

                foreach (T neighbor in Neighbors(current))
                {
                    if (!visited.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
        }

        // Mélységi bejárás implementáció
        public void DFS(T startNode, Action<T> processNode)
        {
            HashSet<T> visited = new HashSet<T>();
            DFSRecursive(startNode, visited, processNode);
        }

        private void DFSRecursive(T current, HashSet<T> visited, Action<T> processNode)
        {
            if (!visited.Contains(current))
            {
                processNode(current);
                visited.Add(current);

                foreach (T neighbor in Neighbors(current))
                {
                    DFSRecursive(neighbor, visited, processNode);
                }
            }
        }

        // Esemény kiváltása
        protected virtual void OnEdgeAdded(T nodeA, T nodeB)
        {
            EdgeAdded?.Invoke(this, new GraphEventArgs<T>(nodeA, nodeB));
        }

        // Új metódus az ismeretség fokának kiszámolásához
        public int DegreeOfConnection(T startNode, T endNode)
        {
            Dictionary<T, int> distances = new Dictionary<T, int>();
            Queue<T> queue = new Queue<T>();

            queue.Enqueue(startNode);
            distances[startNode] = 0;

            while (queue.Count > 0)
            {
                T current = queue.Dequeue();

                foreach (T neighbor in Neighbors(current))
                {
                    if (!distances.ContainsKey(neighbor))
                    {
                        queue.Enqueue(neighbor);
                        distances[neighbor] = distances[current] + 1;

                        if (neighbor.Equals(endNode))
                        {
                            // Útvonal kiírása
                            PrintPath(startNode, endNode, distances);
                            return distances[endNode];
                        }
                    }
                }
            }

            // Ha nem található út
            Console.WriteLine($"No connection found between {startNode} and {endNode}");
            return -1;
        }

        // Útvonal kiírása
        private void PrintPath(T startNode, T endNode, Dictionary<T, int> distances)
        {
            Console.Write($"\nPath from {startNode} to {endNode}: ");
            T current = endNode;

            while (!current.Equals(startNode))
            {
                Console.Write($"{current} <- ");
                foreach (T neighbor in Neighbors(current))
                {
                    if (distances.ContainsKey(neighbor) && distances[neighbor] == distances[current] - 1)
                    {
                        current = neighbor;
                        break;
                    }
                }
            }

            Console.WriteLine(startNode);
        }

    }

    // Személy osztály
    public class Person
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
