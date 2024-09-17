using AbstractModel;
using PostModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostOptimizer
{


    public class PostGraph
    {
        Dictionary<string, PostObject> nodes = new Dictionary<string, PostObject>();
        Dictionary<string, List<TransportBone>> edges = new Dictionary<string, List<TransportBone>>();
        private TaskConfig taskConfig;

        public PostGraph(TaskConfig taskConfig)
        {
            Random rnd = new Random();
            this.taskConfig = taskConfig;

            foreach (var poj in taskConfig.PostObjects)
            {
                nodes.Add(poj.Id, poj);
            }

            foreach(var edge in taskConfig.TransportBones)
            {
                if (!edges.ContainsKey(edge.Start_id))
                    edges.Add(edge.Start_id, new List<TransportBone>());
                edge.Price = rnd.NextDouble();
                edges[edge.Start_id].Add(edge);
            }
        }


        private void AddWays(Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>> ways, 
            string node_from, string node_to, double price, LinkedList<TransportBone> link)
        {
            if (ways[node_from][node_to].price > price)
            {
                ways[node_from][node_to] = (price, link);
            }
        }


        void InitWays(Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>> ways, Dictionary<string, PostObject> nodes)
        {
            foreach (var node1 in nodes.Keys)
            {
                ways.Add(node1, new Dictionary<string, (double price, LinkedList<TransportBone> link)>());
                foreach (var node2 in nodes.Keys)
                {
                    ways[node1].Add(node2, (double.PositiveInfinity, null));
                }
            }
        }

        public void RunOptimizeDeystra()
        {


            Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>> ways =
                new Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>>();

            InitWays(ways, nodes);

            HashSet<string> calculated = new HashSet<string>();

            foreach(var node_from_obj in nodes)
            {
                node_from_obj.Value.Route = new Dictionary<string, Dictionary<string, string?>>();
                node_from_obj.Value.Gates = Array.Empty<string>();


                var current_node = node_from_obj.Key;
                var node_from = node_from_obj.Key;

                //Console.WriteLine($"Calculate node: {current_node}");

                SortedDictionary<double, LinkedList<TransportBone>> graph = new SortedDictionary<double, LinkedList<TransportBone>>();
                graph.Add(0, new LinkedList<TransportBone>());
                while (graph.Count != 0)
                {
                    LinkedList<TransportBone> curent_link = new LinkedList<TransportBone>();
                    var first = graph.First();
                    graph.Remove(first.Key);
                    double price = first.Key;
                    var list = first.Value;

                    if (list.Count == 0)
                        current_node = node_from_obj.Key;
                    else
                    {
                        current_node = list.Last.Value.End_id;
                        curent_link = list;
                    }

                    AddWays(ways, node_from, current_node, price, curent_link);
                    
                    if (false & calculated.Contains(current_node))
                    {
                        foreach (var way in ways[node_from])
                        {
                            double sum_price = price + way.Value.price;
                            if (way.Value.link is null)
                                continue;
                            if (way.Value.link.Count == 0)
                                continue;

                            string node_to = way.Value.link.Last.Value.End_id;
                            if (sum_price < ways[node_from][node_to].price)
                            {
                                LinkedList<TransportBone> new_link = new LinkedList<TransportBone>();
                                foreach (var edge in curent_link)
                                {
                                    new_link.AddLast(edge);
                                }
                                foreach (var edge in way.Value.link)
                                {
                                    new_link.AddLast(edge);
                                }
                                AddWays(ways, node_from, node_to, sum_price, curent_link);
                            }
                        }
                    } else
                    {
                        if (edges.ContainsKey(current_node))
                        {
                            foreach (var edge_add in edges[current_node])
                            {
                                double sum_price = price + edge_add.Price;
                                string node_to = edge_add.End_id;
                                if (sum_price < ways[node_from][node_to].price)
                                {
                                    LinkedList<TransportBone> new_link = new LinkedList<TransportBone>();
                                    foreach (var edge in curent_link)
                                    {
                                        new_link.AddLast(edge);
                                    }
                                    new_link.AddLast(edge_add);
                                    if (!graph.ContainsKey(sum_price))
                                        graph.Add(sum_price, new_link);
                                }
                            }
                        }
                    }
                }
                calculated.Add(node_from);
            }

            double fixPrice = 0;
            foreach (var node in nodes)
            {
                fixPrice += node.Value.FixPrice;
            }

            double transportPrice = 0;
            HashSet<TransportBone> setTranspotBone = new HashSet<TransportBone>();
            StreamWriter outputFile = new StreamWriter("POST-Direction.csv");
            foreach (var way in ways)
            {
                foreach (var w in way.Value)
                {
                    outputFile.WriteLine($"{nodes[way.Key].Index},{nodes[w.Key].Index},{(w.Value.link is null ? 0 : 1)},{(w.Value.link is null ? "-1" : w.Value.link.Count)}");
                    Console.WriteLine($"{way.Key} {w.Key} path {(w.Value.link is null ? "NoWay" : w.Value.link.Count)}");
                    if (!(w.Value.link is null))
                        foreach(var transportBone in w.Value.link)
                        {
                            setTranspotBone.Add(transportBone);
                        }
                }
            }
            outputFile.Close();
            foreach (var transportBone in setTranspotBone)
                transportPrice += transportBone.Price;

            
            Console.WriteLine($"POS Price:{fixPrice} Transport Price:{transportPrice}");

            foreach (var way in ways)
            {
                foreach (var w in way.Value)
                {
                    var link = w.Value.link;


                }
            }




                
        }



    }
}
