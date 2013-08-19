﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Serializer
{
    public class XMLReader : Reader
    {
        private XmlDocument xmlDoc;
        private XmlNode currentNode;

        public XMLReader(StreamReader r)
            : base(r)
        {
            xmlDoc = new XmlDocument();
            currentNode = xmlDoc;
        }

        public override void Start()
        {
            xmlDoc.Load(reader);
        }

        public override Object Read(String fieldName, Type type)
        {
            XmlNode node = currentNode.SelectSingleNode(fieldName);
            if (node == null || node.NodeType != XmlNodeType.Element)
                return null;
            currentNode.RemoveChild(node);
            Object result = "";
            if (type == typeof(byte))
                result = byte.Parse(node.InnerText);
            if (type == typeof(int))
                result = int.Parse(node.InnerText);
            if (type == typeof(long))
                result = long.Parse(node.InnerText);
            if (type == typeof(float))
                result = float.Parse(node.InnerText);
            if (type == typeof(double))
                result = double.Parse(node.InnerText);
            if (type == typeof(decimal))
                result = decimal.Parse(node.InnerText);
            if (type == typeof(bool))
                result = bool.Parse(node.InnerText);
            if (type == typeof(char))
                result = char.Parse(node.InnerText);
            if (type == typeof(String))
                result = node.InnerText;
            return result;
        }

        protected override bool findAndPushGroup(String groupName)
        {
            XmlNode node = currentNode.SelectSingleNode(groupName);
            if (node == null || node.NodeType != XmlNodeType.Element)
                return false;
            currentNode = node;
            return true;
        }
        protected override void popGroup(String groupName)
        {
            XmlNode oldCurrentNode = currentNode;
            currentNode = currentNode.ParentNode;
            currentNode.RemoveChild(oldCurrentNode);
        }
    }
}
