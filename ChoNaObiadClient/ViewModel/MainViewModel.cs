using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ChoNaObiadClient.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 

        public RelayCommand SendMessageCommand { get; private set; }

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                SendMessageCommand = new RelayCommand(SendMessage);
                Message = "Wprowadü treúÊ wiadomoúci...";
                // Code runs "for real"
                IpAdress = "Wprowadü adres IP urzπdzenia...";
            }
        }

        private void SendMessage()
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting.....");
                string hostname = "127.0.0.1";
                tcpclnt.Connect(hostname, 8001);
                // use the ipaddress as in the server program

                DiagnosticMessage = "Connected";
                DiagnosticMessage = "Enter the string to be transmitted : ";

                string str = Message;
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                DiagnosticMessage = ("Transmitting..... ");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);
                char[] receivedMessage = asen.GetChars(bb);
                string receivedStr = "";
                for (int i = 0; i < k; i++)
                {
                    receivedStr += receivedMessage[i];
                }
                DiagnosticMessage += receivedStr;
                tcpclnt.Close();
            }
            catch (Exception e)
            {
                DiagnosticMessage = "Error..... " + e.Message + " " + e.StackTrace;
            }
            DiagnosticMessage += "\nDone!";
        }

        private string _diagnosticMessage;

        public string DiagnosticMessage
        {
            get
            {
                return _diagnosticMessage;
            }
            set
            {
                _diagnosticMessage = value;
                OnPropertyChanged("DiagnosticMessage");
            }
        }


        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private string _ipAdress;

        public string IpAdress
        {
            get
            {
                return (_ipAdress.Count(x => x == '.') == 3 && _ipAdress.Count(x => x < '0' || x > '9' && x != '.') > 0) ? _ipAdress : "$!!!#"; ;
            }
            set
            {
                _ipAdress = value;
                OnPropertyChanged("IpAdress");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}