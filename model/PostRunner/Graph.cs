﻿using PostModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace PostRunner
{
	[XmlRoot(ElementName = "node")]
	public class Node
	{

		[XmlAttribute(AttributeName = "id")]
		public string Id { get; set; }

		[XmlAttribute(AttributeName = "label")]
		public string Label { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlAttribute(AttributeName = "fontsize")]
		public int Fontsize { get; set; }

		[XmlAttribute(AttributeName = "fontname")]
		public string Fontname { get; set; }

		[XmlAttribute(AttributeName = "geo_lat")]
		public double GeoLat { get; set; }

		[XmlAttribute(AttributeName = "geo_lon")]
		public double GeoLon { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }


	}

	[XmlRoot(ElementName = "edge")]
	public class Edge
	{

		[XmlAttribute(AttributeName = "from")]
		public string From { get; set; }

		[XmlAttribute(AttributeName = "to")]
		public string To { get; set; }

		[XmlAttribute(AttributeName = "fontname")]
		public string Fontname { get; set; }

		[XmlAttribute(AttributeName = "fontsize")]
		public int Fontsize { get; set; }

		[XmlAttribute(AttributeName = "label")]
		public string Label { get; set; }

		[XmlAttribute(AttributeName = "type")]
		public string Type { get; set; }
	}

	[XmlRoot(ElementName = "graph")]
	public class Graph
	{
		public Graph() { }
		public Graph(TaskConfig taskConfig, Dictionary<string, string> id2index, Dictionary<string, PostObject> id2poj)
        {
			Graph gr = this;
			gr.Node = new List<Node>();
			gr.Edge = new List<Edge>();
			gr.FileName = "graphs/zone_a_b";
			gr.Rankdir = "LR";
			foreach (var poj in taskConfig.PostObjects)
			{
				if (true | poj.SuType == "B")
				{
					Node nd = new Node();
					nd.Fontname = "Arial";
					nd.Fontsize = 9;
					nd.Id = poj.Index;
					nd.Label = poj.Index;
					nd.Name = poj.Name;
					nd.GeoLat = poj.GeoLat;
					nd.GeoLon = poj.GeoLon;
					nd.Type = poj.SuType;
					gr.Node.Add(nd);
				}
			}
			foreach (var tb in taskConfig.TransportBones)
			{
				Edge edge = new Edge();
				edge.From = id2index[tb.Start_id];
				edge.To = id2index[tb.End_id];
				edge.Fontname = "Arial";
				edge.Fontsize = 9;
				edge.Type = tb.Type;
				gr.Edge.Add(edge);
			}
			//File.WriteAllBytes("zone_a_and_b.xml", Helper.SerializeXML(gr));
			//return;
		}

		[XmlElement(ElementName = "node")]
		public List<Node> Node { get; set; }

		[XmlElement(ElementName = "edge")]
		public List<Edge> Edge { get; set; }

		[XmlAttribute(AttributeName = "file-name")]
		public string FileName { get; set; }

		[XmlAttribute(AttributeName = "rankdir")]
		public string Rankdir { get; set; }


	}

}
