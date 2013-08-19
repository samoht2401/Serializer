using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Serializer
{
    public class XMLWriter : Writer
    {
        private XmlDocument xmlDoc;
        private XmlNode currentNode;

        public XMLWriter(StreamWriter w)
            : base(w)
        {
            xmlDoc = new XmlDocument();
            currentNode = xmlDoc;
        }

        public override void End()
        {
            xmlDoc.Save(writer);
            base.End();
        }

        #region Write

        public override void Write(Field<Object> val)
        {
            XmlElement elem = xmlDoc.CreateElement(val.Name);
            if(val.Value != null)
                elem.InnerText = val.Value.ToString();
            if (val.Commentaire != null && val.Commentaire != "")
                currentNode.AppendChild(xmlDoc.CreateComment(val.Commentaire));
            currentNode.AppendChild(elem);
        }

        #endregion

        #region Group

        protected override void pushGroup(Group group)
        {
            XmlElement elem = xmlDoc.CreateElement(group.Name);
            if (group.Commentaire != null && group.Commentaire != "")
                currentNode.AppendChild(xmlDoc.CreateComment(group.Commentaire));
            currentNode.AppendChild(elem);
            currentNode = elem;
        }
        protected override void popGroup(Group group)
        {
            currentNode = currentNode.ParentNode;
        }

        #endregion
    }
}
