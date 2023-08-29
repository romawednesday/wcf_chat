using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace wcf_chat
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        private List<ServerUser> users = new List<ServerUser>(); // List stores connected users
        private int nextID = 1; // Initialize an identifier for the user

        // Implementation of the Connect operation contract.
        public int Connect(string name)
        {
            if (users.Any(u => u.Name == name)) // Check if the username is already taken
            {
                throw new FaultException("Username already taken. Please choose a different username.");
            }

            // Create a new ServerUser instance to represent the connected user
            ServerUser user = new ServerUser()
            {
                ID = nextID,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextID++; // Increment the identifier for the next user
            SendMsg($": {user.Name} was connected to the chat!", 0); // Notify users about the new connection
            users.Add(user); // Add the user to the list of connected users
            return user.ID; 
        }

        // Implementation of the Disconnect operation contract
        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(x => x.ID == id); // Find the user to disconnect
            if (user != null)
            {
                users.Remove(user); // Remove the user from the list of connected users
                SendMsg($": {user.Name} was disconnected from the chat!", 0); // Notify other users about the disconnection
            }
        }

        // Implementation of the SendMsg operation contract
        public void SendMsg(string msg, int id)
        {
            foreach (var u in users) 
            {
                string answer = DateTime.Now.ToShortTimeString();

                var user = users.FirstOrDefault(x => x.ID == id); // Find the user sending the message.
                if (user != null)
                {
                    answer += $": {user.Name} ";
                }

                answer += "-> " + msg;

                // Send the message to the callback channel of each user
                u.operationContext.GetCallbackChannel<IServiceCallback>().MsgCallback(answer);
            }
        }
    }
}
