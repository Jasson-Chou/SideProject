using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IPCService
{
    public delegate void OnIPCClientReadHandler(byte[] data);
    public class Client
    {
        private byte[] buffer = new byte[TCPSpec.Instance.BufferTotalBytes];
        private NetworkStream networkStream;
        private TcpClient tcpClient;
        private Task ReadListenerHandlerTask;
        private System.Threading.CancellationTokenSource ReadListenerHandleCancelTokenSource;
        private System.Threading.CancellationToken ReadListenerHandleCancelToken;
        public bool IsOpen { get; private set; }

        public event OnIPCClientReadHandler OnIPCRead = null;
        public Client()
        {
            IsOpen = false;
            ReadListenerHandleCancelTokenSource = new System.Threading.CancellationTokenSource();
            ReadListenerHandleCancelToken = ReadListenerHandleCancelTokenSource.Token;
            ReadListenerHandlerTask = new Task(ReadListenerHandler, ReadListenerHandleCancelToken);
            tcpClient = new TcpClient();
            tcpClient.ReceiveBufferSize = TCPSpec.Instance.BufferBytes;
        }

        public void Open()
        {
            tcpClient.Connect(TCPSpec.Instance.IPCIPAddress, TCPSpec.Instance.Port);
            networkStream = tcpClient.GetStream();
            IsOpen = true;
            try
            {
                ReadListenerHandlerTask.Start();
            }
            catch (OperationCanceledException oce)
            {
#if DEBUG
                Debug.WriteLine($"Cancel Read Listener Handler.{Environment.NewLine}[{oce.Message}]");
#endif
            }
            finally
            {

            }

        }

        public void Close()
        {
            IsOpen = false;
            ReadListenerHandleCancelTokenSource.Cancel();
        }

        public bool Write(byte[] data)
        {
            TCPSpec.Instance.TransferSpecConvert(data, out byte[] buffer);
            try
            {
                networkStream.Write(buffer, 0, TCPSpec.Instance.BufferTotalBytes);
            }
            catch (ArgumentNullException ane)
            {
#if DEBUG
                Debug.WriteLine(ane.Message);
#endif
                return false;
            }
            catch (ArgumentOutOfRangeException aore)
            {
#if DEBUG
                Debug.WriteLine(aore.Message);
#endif
                Close();
                return false;
            }
            catch (System.IO.IOException ioe)
            {
#if DEBUG
                Debug.WriteLine(ioe.Message);
#endif
                Close();
                return false;
            }
            catch (ObjectDisposedException ode)
            {
#if DEBUG
                Debug.WriteLine(ode.Message);
#endif
                Close();
                return false;
            }

            return true;
        }

        private void ReadListenerHandler()
        {
            try
            {
                do
                {
                    if (tcpClient.Client.Poll(100, SelectMode.SelectRead))
                    {
                        buffer.Fill((byte)0);
                        networkStream.BeginRead(buffer, 0, TCPSpec.Instance.BufferTotalBytes, (ar) =>
                        {
                            var clientSocket = (Socket)ar.AsyncState;
                            int receiveLen = clientSocket.EndReceive(ar, out SocketError socketError);
                            if (receiveLen != TCPSpec.Instance.BufferTotalBytes)
                            {
#if DEBUG
                                Debug.WriteLine($"Recevice Length Error : {receiveLen} bytes, socket error {socketError}");
#endif
                                return;
                            }

                            TCPSpec.Instance.ReceiveSpecConvert(buffer, out byte[] data);
                            OnIPCRead?.Invoke(data);

                        }, tcpClient.Client);
                    }
                    else if (ReadListenerHandleCancelToken.IsCancellationRequested)
                    {
                        ReadListenerHandleCancelToken.ThrowIfCancellationRequested();
                    }
                } while (IsOpen);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e.Message);
#endif
                Close();
            }
        }

        public void Dispose()
        {
            ReadListenerHandlerTask.Dispose();
            ReadListenerHandleCancelTokenSource.Dispose();
        }
    }

    

    
}
