"""
Author Amit Khatri
Description:
    This is an example script for TCP client
#  https://stackoverflow.com/questions/11460788/how-to-start-a-child-process-and-use-it-as-a-server-in-python
"""

import setup_environment
from MyTCP import TCPClient
import subprocess
from IPCService import IPCClient

# is shell = True slave.kill does not work
prog = subprocess.Popen('python testgui.py', shell=False, stdin=subprocess.PIPE, stdout=subprocess.PIPE, stderr=subprocess.PIPE)


client = TCPClient()
VCipc = IPCClient()
VCipc.Connect(8192)
while True:
    # Next line implement move to die method
    # Move to contact
    # send "1,2,3,4"
    #     or "0,0,0,0"
    #     or -1 to exit out of the loop as well as tell server that
    #        you are done testing
    #
    # Giga case: Move to next die, Contact 
    # if done testing all dies send -1
    #data = input("Enter a string to start test (-1 to quit):")
    data = VCipc.Recv()
    print("\nsending data", data)

    # ###################################
    # send data to server to collection 
    # Giga will develop
    # ################################
    client.send(data)

    # #############################################################
    # one data are send wait for test to complete and receive data
    # ############################################################
    testdata = client.recv()

    VCipc.Send(testdata)

    # just prining data
    print("receive testdata:", testdata)
    print("\n\n")
    if data == '-1':
        break

# ########################
# End close the socket 
# connection
# ########################
VCipc.Disconnect()
client.close()
prog.kill()
