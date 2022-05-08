using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPCService
{
    public delegate void OnIPCServerReadHandler(byte[] data, IClientPackage clientPackage);
    public class IPCServer : IDisposable
    {
        private TcpListener tcpListener;
        private Task AcceptSocketHandlerTask;
        public bool IsOpen { get; private set; }
        private List<ClientPackage> ClientPackages;
        public event OnIPCServerReadHandler OnIPCRead = null;
        public IPCServer()
        {
            IsOpen = false;
            ClientPackages = new List<ClientPackage>(8);

            tcpListener = new TcpListener(TCPSpec.Instance.IPCIPAddress, TCPSpec.Instance.Port);

            AcceptSocketHandlerTask = new Task(AcceptSocketHandler);
        }

        public void Open()
        {
            IsOpen = true;
            tcpListener.Start();
            AcceptSocketHandlerTask.Start();
        }

        

        public bool Write(byte[] data, IClientPackage package)
        {
            TCPSpec.Instance.TransferSpecConvert(data, out byte[] buffer);
            return package.Write(buffer);
        }

        private void AcceptSocketHandler()
        {
            do
            {
                Socket accpectSocket = null;
                try
                {
                    accpectSocket = tcpListener.AcceptSocket();
                }
                catch (InvalidOperationException ioe)
                {
#if DEBUG
                    Debug.WriteLine(ioe.Message);
#endif
                    return;
                }
                ClientPackage clientPackage = new ClientPackage(accpectSocket, TCPSpec.Instance.BufferTotalBytes, ReadListenerHandler);
                clientPackage.Start();

            } while (IsOpen);
        }

        private void ReadListenerHandler(ClientPackage clientPackage)
        {
            Socket socket = clientPackage.Socket;

            try
            {
                do
                {

                    if (socket.Poll(100, SelectMode.SelectRead))
                    {
                        clientPackage.Buffer.Fill((byte)0);
                        socket.BeginReceive(clientPackage.Buffer, 0, TCPSpec.Instance.BufferTotalBytes, SocketFlags.None, (ar) =>
                        {
                            ClientPackage package = (ClientPackage)ar.AsyncState;
                            int receiveLen = socket.EndReceive(ar, out SocketError socketError);
                            if (receiveLen != TCPSpec.Instance.BufferTotalBytes)
                            {
#if DEBUG
                                Debug.WriteLine($"Recevice Length Error : {receiveLen} bytes, socket error {socketError}");
#endif
                                return;
                            }

                            TCPSpec.Instance.ReceiveSpecConvert(package.Buffer, out byte[] data);
                            OnIPCRead?.Invoke(data, clientPackage);
                        }, clientPackage);
                    }
                    else if (clientPackage.ReadListenerHandleCancelToken.IsCancellationRequested)
                    {
                        clientPackage.ReadListenerHandleCancelToken.ThrowIfCancellationRequested();
                        break;
                    }
                } while (IsOpen);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                clientPackage.Close();
            }

        }

        public void Close()
        {
            IsOpen = false;
            ClientPackages.Close();
        }

        public void Dispose()
        {
            Close();
            tcpListener.Stop();
        }
    }
}
