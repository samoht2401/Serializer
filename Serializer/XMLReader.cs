using System;
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
        private bool cantLoad;

        public XMLReader(StreamReader r)
            : base(r)
        {
            xmlDoc = new XmlDocument();
            currentNode = xmlDoc;
            cantLoad = false;
        }

        public override void Start()
        {
            try {
                xmlDoc.Load(reader);
            }
            catch (Exception ex) 
            {
                cantLoad = true;
            }
        }

        public override Object Read(String fieldName, Type type)
        {
            if (cantLoad)
                return null;
            XmlNode node = currentNode.SelectSingleNode(fieldName);
            if (node == null || node.NodeType != XmlNodeType.Element)
                return null;
            currentNode.RemoveChild(node);
            Object result = "";
            if (type == typeof(byte))
                result = byte.Parse(node.InnerText);
            else if (type == typeof(int))
                result = int.Parse(node.InnerText);
            else if (type == typeof(long))
                result = long.Parse(node.InnerText);
            else if (type == typeof(float))
                result = float.Parse(node.InnerText);
            else if (type == typeof(double))
                result = double.Parse(node.InnerText);
            else if (type == typeof(decimal))
                result = decimal.Parse(node.InnerText);
            else if (type == typeof(bool))
                result = bool.Parse(node.InnerText);
            else if (type == typeof(char))
                result = char.Parse(node.InnerText);
            else if (type == typeof(String))
                result = node.InnerText;
            else if (type.IsEnum)
                result = Enum.Parse(type, node.InnerText);
            return result;
        }

        protected override bool findAndPushGroup(String groupName)
        {
            if (cantLoad)
                return false;
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
