using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serializer
{
    public class Group
    {
        private string name;
        private string commentaire;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Commentaire
        {
            get { return commentaire; }
            set { commentaire = value; }
        }

        public Group(string name, string commentaire)
        {
            this.name = name;
            this.commentaire = commentaire;
        }
    }
}
