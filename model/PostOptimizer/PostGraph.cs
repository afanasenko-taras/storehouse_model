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


        public void RunOptimizeDeystra()
        {
            Dictionary<string, Dictionary<string, LinkedList<TransportBone>>> ways = new Dictionary<string, Dictionary<string, LinkedList<TransportBone>>>();

            foreach(var node_from in nodes)
            {
                ways.Add(node_from.Key, new Dictionary<string, LinkedList<TransportBone>>());
                var current_node = node_from.Key;
                foreach (var node_to in nodes)
                {
                    LinkedList<TransportBone> curent_link = new LinkedList<TransportBone>();
                    Dictionary<double, LinkedList<TransportBone>> graph = new Dictionary<double, LinkedList<TransportBone>>();
                    graph.Add(0, new LinkedList<TransportBone>());
                    while (current_node!=node_to.Key & graph.Count!=0)
                    {
                        var first = graph.First();
                        graph.Remove(first.Key);
                        var weight = first.Key;
                        var list = first.Value;
                        if (list.Count == 0)
                            current_node = node_from.Key;
                        else
                        {
                            current_node = list.Last.Value.End_id;
                            curent_link = list;
                        }

                        if (ways.ContainsKey(current_node) && ways[current_node].ContainsKey(node_to.Key))

                        if (edges.ContainsKey(current_node)) {
                            foreach (var edge_add in edges[current_node])
                            {
                                LinkedList<TransportBone> new_link = new LinkedList<TransportBone>();
                                foreach (var edge in curent_link)
                                {
                                    new_link.AddLast(edge);
                                }
                                new_link.AddLast(edge_add);
                                graph.Add(weight + edge_add.Price, new_link);
                            }
                        }
                    }
                    if (current_node == node_to.Key)
                        ways[node_from.Key].Add(node_to.Key, curent_link);
                    else
                        ways[node_from.Key].Add(node_to.Key, null); 
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
                    Console.WriteLine($"{way.Key} {w.Key} path {(w.Value is null ? "NoWay" : w.Value.Count)}");
                    if (!(w.Value is null))
                        foreach(var transportBone in w.Value)
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
