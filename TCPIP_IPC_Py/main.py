from IPCService import IPCClient




# Press the green button in the gutter to run the script.
if __name__ == '__main__':
    #s = "Hello\\nWorld"
    #s.encode()
    #print(s.encode())
    #print(s.encode().decode("unicode-escape"))
    ic = IPCClient()
    ic.Connect(8192)
    print('Connected Server')
    print(f'Recv:' + ic.Recv())
    ic.Send("Hello Server\0")
    ic.Disconnect()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
