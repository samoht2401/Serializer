using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Serializer
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ToSerializeAttribute : Attribute
    {
        public String Commentaire = null;
        public String SpecialName = null;
    }
}
