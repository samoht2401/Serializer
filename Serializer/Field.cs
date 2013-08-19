using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serializer
{
    public class Field<T>
    {
        private string name;
        private T value;
        private string commentaire;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public T Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public string Commentaire
        {
            get { return commentaire; }
            set { commentaire = value; }
        }

        public Field(string name, T value, string commentaire = "")
        {
            this.name = name;
            this.value = value;
            this.commentaire = commentaire;
        }
    }
}
