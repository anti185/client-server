using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

       
        ClientManager clientManager;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            IPHostEntry entry = Dns.Resolve("localhost");
            IPAddress ip = entry.AddressList[0];
            clientManager = new ClientManager();
            clientManager.Connected += connection;
            clientManager.Received += received;
            clientManager.Connect(Convert.ToString(ip), 11000);
        }
        public void connection(object sender, EventArgs e)
        {

        }

        public void received(ClientManager client, string data)
        {


            Dispatcher.BeginInvoke(new ThreadStart(delegate
            {
                Password.Text += data + "\n\r";
            }));
            

        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            clientManager.Send("privet server");
        }
    }
}
