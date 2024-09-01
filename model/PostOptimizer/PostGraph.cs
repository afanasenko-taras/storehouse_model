using AbstractModel;
using PostModel;
using System;
using System.Collections.Generic;
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
            this.taskConfig = taskConfig;

            foreach (var poj in taskConfig.PostObjects)
            {
                nodes.Add(poj.Id, poj);
            }

            foreach(var edge in taskConfig.TransportBones)
            {
                if (!edges.ContainsKey(edge.Start_id))
                    edges.Add(edge.Start_id, new List<TransportBone>());
                edges[edge.Start_id].Add(edge);
            }
        }


        private void AddWays(Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>> ways, 
            string node_from, string node_to, double price, LinkedList<TransportBone> link)
        {
            if (!ways[node_from].ContainsKey(node_to))
                ways[node_from].Add(node_to, (double.PositiveInfinity, null));

            if (ways[node_from][node_to].price > price)
            {
                ways[node_from][node_to] = (price, link);
            }
        }

        public void RunOptimizeDeystra()
        {


            Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>> ways =
                new Dictionary<string, Dictionary<string, (double price, LinkedList<TransportBone> link)>>();

            foreach(var node_from in nodes)
            {
                var current_node = node_from.Key;
                if (!ways.ContainsKey(current_node))
                    ways.Add(current_node, new Dictionary<string, (double price, LinkedList<TransportBone> link)>());
                foreach (var node_to in nodes)
                {
                    LinkedList<TransportBone> curent_link = new LinkedList<TransportBone>();
                    double price = double.PositiveInfinity; 
                    SortedDictionary<double, LinkedList<TransportBone>> graph = new SortedDictionary<double, LinkedList<TransportBone>>();
                    graph.Add(0, new LinkedList<TransportBone>());
                    while (graph.Count!=0)
                    {
                        var first = graph.First();
                        graph.Remove(first.Key);
                        price = first.Key;
                        var list = first.Value;

                        if (list.Count == 0)
                            current_node = node_from.Key;
                        else
                        {
                            current_node = list.Last.Value.End_id;
                            curent_link = list;
                        }

                        if (node_to.Key == current_node)
                            AddWays(ways, node_from.Key, node_to.Key, price, curent_link);


                        if (!ways.ContainsKey(current_node))
                            ways.Add(current_node, new Dictionary<string, (double price, LinkedList<TransportBone> link)>());

                        if (ways[current_node].ContainsKey(node_to.Key)) {
                            double sum_price = ways[node_from.Key][node_to.Key].price + price;
                            if (ways[node_from.Key].ContainsKey(node_to.Key)) {
                                if (ways[node_from.Key][node_to.Key].price <= sum_price)
                                    continue;
                            }

                            LinkedList<TransportBone> new_link = new LinkedList<TransportBone>();
                            foreach (var edge in curent_link)
                            {
                                new_link.AddLast(edge);
                            }
                            foreach (var edge in ways[node_from.Key][node_to.Key].link)
                            {
                                new_link.AddLast(edge);
                            }
                            ways[node_from.Key][node_to.Key] = (sum_price, new_link);
                        } else
                        {
                            if (edges.ContainsKey(current_node))
                            {
                                foreach (var edge_add in edges[current_node])
                                {
                                    LinkedList<TransportBone> new_link = new LinkedList<TransportBone>();
                                    foreach (var edge in curent_link)
                                    {
                                        new_link.AddLast(edge);
                                    }
                                    new_link.AddLast(edge_add);
                                    graph.Add(price + edge_add.Price, new_link);
                                }
                            }
                        }

                    }
                    if (current_node == node_to.Key)
                        if (ways[node_from.Key][node_to.Key].price > price)
                            ways[node_from.Key][node_to.Key] = (price, curent_link);

                }
            }

            double fixPrice = 0;
            foreach (var node in nodes)
            {
                fixPrice += node.Value.FixPrice;
            }

            double transportPrice = 0;
            HashSet<TransportBone> setTranspotBone = new HashSet<TransportBone>();
            foreach (var way in ways)
            {
                foreach (var w in way.Value)
                {
                    Console.WriteLine($"{way.Key} {w.Key} path {(w.Value.link is null ? "NoWay" : w.Value.link.Count)}");
                    if (!(w.Value.link is null))
                        foreach(var transportBone in w.Value.link)
                        {
                            setTranspotBone.Add(transportBone);
                        }
                }
            }

            foreach (var transportBone in setTranspotBone)
                transportPrice += transportBone.Price;

            Console.WriteLine($"POS Price:{fixPrice} Transport Price:{transportPrice}");


        }



    }
}
