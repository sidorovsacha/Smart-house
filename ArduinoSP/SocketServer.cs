﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Newtonsoft.Json;
 

namespace ArduinoSP
{
     class SocketServer
     {
         public class StateObject {
             public Socket workSocket = null;
             public const int BufferSize = 1024;
             public byte[] buffer = new byte[BufferSize];
             // Received data string.
             public StringBuilder sb = new StringBuilder();
         }
        public static string data = "";
        public static byte[] cldata = new byte[1024];
        public static void ReceiveCallback(IAsyncResult AsyncCall)
        {
            allDone.Set();
            var DatajsonGuest = new ArduinoSP.DictionaryJson();
            {
                DatajsonGuest.temperDS18b20 = Form1.mas[2];
                DatajsonGuest.pressureBMP085 = Form1.mas[3];
                DatajsonGuest.temperBMP085 = Form1.mas[4];
                DatajsonGuest.humidityDHT22 = Form1.mas[5];
                DatajsonGuest.temperDHT22 = Form1.mas[6];
            }
            var jsondat = JsonConvert.SerializeObject(DatajsonGuest);
            //SocketAsyncEventArgs e = new SocketAsyncEventArgs();

            int i = 0;
            

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

           
            Socket listener = (Socket)AsyncCall.AsyncState;
            Socket client = listener.EndAccept(AsyncCall);
        
           
           try{

               while (true)
               {

                   i = client.Receive(cldata);
                   Byte[] message = encoding.GetBytes(jsondat);
                   if (i > 0)
                   {
                     
                       data = Encoding.UTF8.GetString(cldata, 0, i);


                       if (data == "END")
                       {
                           Byte[] messagуe = encoding.GetBytes("ClientDisconnekt");
                           client.Send(messagуe);
                           client.Close();
                           break;

                       }
                       client.Send(message);
                   }
               } 
             
               
            }
         

            catch (SocketException){
          }
         catch (Exception) { }


            
            // client.Close();

            // После того как завершили соединение, говорим ОС что мы готовы принять новое
         //   listener.BeginAccept(new AsyncCallback(ReceiveCallback), listener);
        }

        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }
         
        
        public static void Socket()
        {  

            
            try
            {
           
                IPAddress localAddress = IPAddress.Parse("127.0.0.1");

                Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint ipEndpoint = new IPEndPoint(localAddress, 2200);

                
              
                listenSocket.Bind(ipEndpoint);
                 
                listenSocket.Listen(1);

               
               
                while (true)
                {
                    //if (Form1.StatusServer == false) { listenSocket.Close(); }
                    allDone.Reset();
                    listenSocket.BeginAccept(new AsyncCallback(ReceiveCallback), listenSocket);

                    allDone.WaitOne();
                }
                

            }
            catch (Exception)
            {
              
            }
        }
    }
    
}
