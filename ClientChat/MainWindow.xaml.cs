using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
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

namespace ClientChat
{
    public class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }

        public MessageEventArgs(string message)
        {
            Message = message;
        }
    }

    public partial class MainWindow : Window, ServiceChat.IService1Callback
    {
        private bool isConnected = false;
        private ServiceChat.Service1Client client;
        private int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Method to connect the user to the chat
        private void ConnectUser()
        {
            if (!isConnected)
            {
                try
                {
                    client = new ServiceChat.Service1Client(new System.ServiceModel.InstanceContext(this));
                    ID = client.Connect(UserName.Text); // Connect user and get an ID
                    UserName.IsEnabled = false;
                    Connecter.Content = "Disconnect";
                    isConnected = true;
                }
                catch (FaultException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Method to disconnect the user from the chat
        private void DisconnectUser()
        {
            if (isConnected)
            {
                client.Disconnect(ID); // Disconnect the user using their ID
                client = null;
                UserName.IsEnabled = true;
                Connecter.Content = "Connect";
                isConnected = false;
            }
        }

        // Event handler for the Connect/Disconnect button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else
            {
                ConnectUser();
            }
        }

        // Event handler for the Clear Chat button
        private void ClearChatButton_Click(object sender, RoutedEventArgs e)
        {
            Chat.Items.Clear();
        }

        // Implementation of the callback interface to receive messages from the service
        public void MsgCallback(string msg)
        {
            Chat.Items.Add(msg);
            Chat.ScrollIntoView(Chat.Items[Chat.Items.Count - 1]); // Scrolls to the latest message
        }

        // Event handler for the window closed event to ensure disconnection
        private void Window_Closed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DisconnectUser();
        }

        // Event handler for sending messages when the Enter key is pressed
        private void Texting(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (client != null)
                {
                    client.SendMsg(Message.Text, ID); // Send the message using the client instance and user's ID
                    Message.Text = string.Empty; // Clear the message input
                }
            }
        }
    }
}
