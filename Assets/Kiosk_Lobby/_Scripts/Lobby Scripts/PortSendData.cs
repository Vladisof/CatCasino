using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class PortSendData : MonoBehaviour
{
	#region private members 	
	/// <summary> 	
	/// TCPListener to listen for incomming TCP connection 	
	/// requests. 	
	/// </summary> 	
	private TcpListener tcpListener;
	/// <summary> 
	/// Background thread for TcpServer workload. 	
	/// </summary> 	
	private Thread tcpListenerThread;
	/// <summary> 	
	/// Create handle to connected tcp client. 	
	/// </summary> 	
	private TcpClient connectedTcpClient;
	#endregion

	public static PortSendData instance;

	//public static float amounttocollect;
	// Use this for initialization
	void Start()
	{
		instance = this;
		// Start TcpServer background thread 		
		tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
		tcpListenerThread.IsBackground = true;
		tcpListenerThread.Start();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SendMessage(5);
		}

	}

	/// <summary> 	
	/// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
	/// </summary> 	
	private void ListenForIncommingRequests()
	{
		try
		{
			// Create listener on localhost port 8052. 			
			tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8082);
			tcpListener.Start();
			Debug.Log("Server is listening");
			Byte[] bytes = new Byte[1024];
			while (true)
			{
				using (connectedTcpClient = tcpListener.AcceptTcpClient())
				{
					// Get a stream object for reading

					using (NetworkStream stream = connectedTcpClient.GetStream())
					{
						int length;
						// Read incomming stream into byte arrary. 						
						while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
						{
							var incommingData = new byte[length];
							Array.Copy(bytes, 0, incommingData, 0, length);
							// Convert byte array to string message. 							
							string clientMessage = Encoding.ASCII.GetString(incommingData);
							Debug.Log("client message received as: " + clientMessage);
							

						}
					}
				}
			}
			//while (true)//&&senddata==1)
			//{
			//	senddata = 0;
			//	using (connectedTcpClient = tcpListener.AcceptTcpClient())
			//	{

			//		//  Get a stream object for reading
			//		NetworkStream stream1 = connectedTcpClient.GetStream();
			//		if (stream1.CanWrite)
			//		{
			//			string serverMessage = "Amount " + amounttocollect.ToString() + "      Time  " + System.DateTime.Now.ToString();
			//			// Convert string message to byte array.                 
			//			byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
			//			// Write byte array to socketConnection stream.               
			//			stream1.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
			//			Debug.Log("Server sent his message - should be received by client");
			//		}

			//	}
			//}
		}
		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}
	}
	public void SendMessage(int amounttocollect)
	{

		if (connectedTcpClient == null)
		{
			Debug.Log("Not connected");
			return;
		}

		try
		{

			//  Get a stream object for reading
			NetworkStream stream1 = connectedTcpClient.GetStream();
			if (stream1.CanWrite)
			{
				string serverMessage = "Amount " + amounttocollect.ToString() + "  Date " + System.DateTime.Now.ToString();
				// Convert string message to byte array.                 
				byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
				// Write byte array to socketConnection stream.               
				stream1.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
				Debug.Log("Server sent his message - should be received by client");
			}

		}

		catch (SocketException socketException)
		{
			Debug.Log("SocketException " + socketException.ToString());
		}

	}	
}

	/// <summary> 	
	/// Send message to client using socket connection. 	
	/// </summary> 	
	