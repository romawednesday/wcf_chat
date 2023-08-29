using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel; 
using System.Text;

namespace wcf_chat
{
    // Define a service contract for the chat service
    [ServiceContract(CallbackContract = typeof(IServiceCallback))] // Specify the callback contract for handling callbacks to clients
    public interface IService1
    {
        [OperationContract]
        int Connect(string name); // Operation contract to handle client connection -> returns an identifier for the client

        [OperationContract]
        void Disconnect(int id); // Operation contract to handle client disconnection

        [OperationContract(IsOneWay = true)] // One-way operation contract (does not wait for a response) to send messages from client to server
        void SendMsg(string msg, int id); // Operation contract to send messages from clients to the server
    }

    // Define a callback contract for the client to receive messages from the server 
    public interface IServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void MsgCallback(string msg); // Operation contract for the server to send messages back to the clients
    }
}
