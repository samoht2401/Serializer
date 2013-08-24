using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Serializer;

namespace Test
{
    class TestClass
    {
        public class SubTest
        {
            [ToSerialize]
            public int nb;

            private SubTest()
            {
                nb = 0;
            }

            public SubTest(int nb)
            {
                this.nb = nb;
            }
        }

        public enum Lang
        {
            Hello,
            Hola
        };

        [ToSerialize]
        public int entier;

        [ToSerialize(Commentaire = "Un floattant")]
        public float flotant;

        [ToSerialize(Commentaire = "Voici une list de correspondances")]
        public Dictionary<float, string> dico;

        [ToSerialize(Commentaire = "Voici une list")]
        public List<SubTest> list;

        //[ToSerialize(Commentaire = "Et ici une class", SpecialName = "Coucou")]
        public TestClass uneClass;

        [ToSerialize]
        public string chaine;

        [ToSerialize]
        private Lang language = Lang.Hello;

        public TestClass(TestClass c)
        {
            entier = 4;
            flotant = 0.305f;
            dico = new Dictionary<float, string>();
            dico.Add(0.2f, "20%");
            dico.Add(0.1f, "10%");
            dico.Add(0.3f, "30%");
            dico.Add(0.8f, "80%");
            dico.Add(0.6f, "60%");
            dico.Add(0.7f, "70%");
            dico.Add(0.5f, "50%");
            dico.Add(0.9f, "90%");
            dico.Add(0.4f, "40%");
            dico.Add(1f, "100%");
            list = new List<SubTest>();
            list.Add(new SubTest(5));
            list.Add(new SubTest(8));
            uneClass = c;
            chaine = "Salut!^^";
        }
    }
}
