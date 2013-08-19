using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Serializer;

namespace Test
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SerializeManager manager = new SerializeManager();
        TestClass testClass = new TestClass(new TestClass(null));

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            FileStream stream = File.Create("C:\\Users\\Thomas\\Documents\\Visual Studio 2010\\Projects\\Serializer\\Test\\bin\\Debug\\test.txt");
            StreamWriter writer = new StreamWriter(stream);
            manager.Serialize<TestClass>(testClass, "Test", new XMLWriter(writer));
            writer.Flush();
            writer.Close();
        }

        private void Button_Click_Load(object sender, RoutedEventArgs e)
        {
            FileStream stream = File.OpenRead("C:\\Users\\Thomas\\Documents\\Visual Studio 2010\\Projects\\Serializer\\Test\\bin\\Debug\\test.txt");
            StreamReader reader = new StreamReader(stream);
            manager.Unserialize<TestClass>(testClass, "Test", new XMLReader(reader));
            reader.Close();

            textBoxEntier.Text = testClass.entier.ToString();
        }

        private void TextBoxEntier_TextChanged(object sender, TextChangedEventArgs e)
        {
            testClass.entier = int.Parse(textBoxEntier.Text);
        }
    }
}
