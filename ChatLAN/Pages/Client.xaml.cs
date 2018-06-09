using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
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
using System.Xml.Serialization;
using ChatLAN.Utils;

namespace ChatLAN.Pages
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private ObjClient _thisClient;
        //private ChatClient chatClient;
        private string _name = "Gef";
        private string _passHash;
        public string Pass
        {
            set
            {
                MD5 md5 = MD5.Create();
                byte[] input = Encoding.UTF8.GetBytes(value);
                byte[] hash = md5.ComputeHash(input);
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var @byte in hash)
                    stringBuilder.Append(@byte.ToString("X2"));
                
                _passHash = stringBuilder.ToString();
            }
        }

        private static TextBox _tbSend;

        static string userName;
        private const string host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient client;
        static NetworkStream stream;

        public Client(string userName, string pass)
        {
            InitializeComponent();
            MainWindow.Close += (sender, args) => Disconnect();
            Pass = pass;
           // string sendMessage = "\n\n" + userName + ' '+ _passHash;
            //chatClient = new ChatClient();
            //chatClient.Connect();
            //_tbSend = TbSend;
            // Server.printText("Введите свое имя: ");
            //userName = _name; //Console.ReadLine();

            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток
                StreamOfClient.Join(stream, new Signature(userName,_passHash, Util.TypeMessage.SignIn));
                
                //  //string message = userName;
                //  byte[] data = Encoding.UTF8.GetBytes(sendMessage);
                ////  MessageBox.Show(message);
                ////  stream.Write(data, 0, data.Length);
                //  XmlSerializer serializer = new XmlSerializer(typeof(ObjClient));
                //  _thisClient=(ObjClient)serializer.Deserialize(client.GetStream());

                // string temp = readMessage(stream, client.Available);
                //int t = client.Available;
                // запускаем новый поток для получения данных
                //  Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                //  receiveThread.Start(); //старт потока
                // Server.printText("Добро пожаловать, {0}"+ userName);
                //SendMessage();
                //using (FileStream fs = new FileStream("obj.xml", FileMode.OpenOrCreate))
                //{
                //    byte[] buffer = new byte[64];
                //    do
                //    {
                //        stream.Read(data, 0, data.Length);
                //        fs.Write(buffer, 0, buffer.Length);
                //    } while (stream.DataAvailable);
                //    // fs.Write();
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //finally
            //{
            //    Disconnect();
            //}
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }


        public string readMessage(NetworkStream stream, int size)
        {
            try
            {
                byte[] data = new byte[64]; // буфер для получаемых данных
                StringBuilder builder = new StringBuilder();
                int bufferSize = 0;
                do
                {
                    int tempSize = stream.Read(data, 0, data.Length);
                    bufferSize += tempSize;
                    builder.Append(Encoding.UTF8.GetString(data, 0, tempSize));
                } while (bufferSize != size);
                //for (int i=0; i !=-1 ;)
                //{
                //    i = stream.Read(data, 0, data.Length);
                //    builder.Append(Encoding.UTF8.GetString(data));
                //}
                //int bytes = 0;
                //data = stream.ReadBytes(64);
                //while (stream.PeekChar() > -1)
                //{
                //    data = stream.ReadBytes(data.Length);
                //    builder.Append(Encoding.UTF8.GetString(data, offset, bytes));
                //}
                //do
                //{

                //   // bytes = stream.ReadBlock(data, 0, data.Length);

                //} while (stream.DataAvailable);

                return builder.ToString(); //вывод сообщения
            }
            catch
            {
                Console.WriteLine("Подключение прервано!"); //соединение было прервано
                Console.ReadLine();
                Disconnect();
                return null;
            }
        }

        // отправка сообщений
        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            //while (true)
            //{
            string message = _tbSend.Text;
            byte[] data = Encoding.Unicode.GetBytes(message);
            stream.Write(data, 0, data.Length);
            //}
        }

        // получение сообщений
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    } while (stream.DataAvailable);

                    string message = builder.ToString();
                    MessageBox.Show(message);
                    //Console.WriteLine(message); //вывод сообщения
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            //if (stream != null)
            stream?.Close(); //отключение потока
            // if (client != null)
            client?.Close(); //отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}