import socket

class IPCClient:
    def __init__(self):
        self.service = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def Connect(self, PORT):
        self.service.connect(("localhost", PORT))

    def Disconnect(self):
        self.service.close()

    def Send(self, data):
        if (data in [None, ""]):
            print("Can not send data which is None or empty")
            print("Sending -1, exiting")
            data = '-1'
        data = bytes(data, 'utf-8')
        self.service.send(data)

    def Recv(self):
        data = self.service.recv(124000)
        data = data.decode('utf-8')
        return data