import socket


LOCAL_IP = ''
LOCAL_PORT = 101010

REMOTE_SERVER_IP = ''
REMOTE_SERVER_PORT = 110101

def request_connection():
    pass

def receive_connection():
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.connect((REMOTE_SERVER_IP, REMOTE_SERVER_PORT))
        s.sendall(str.encode(f'{LOCAL_IP},{LOCAL_PORT}'))

        # This properly does not work properly
        reply = repr(s.recv(1024))
        split_reply = reply.split(',')
        peer_ip = split_reply.index(0)
        peer_port = split_reply.index(1)

        s.close()
        s.connect((peer_ip, peer_port))

def run_middleware():
    while True:
        print('What do you want to do?')
        print('\t1: Initiate a connection')
        print('\t2: Wait for connection request')
        print('\tq: For quitting')
        u_input = input('Choice: ')

        if u_input == '1':
            request_connection()
        elif u_input == '2':
            receive_connection()
        elif u_input == 'q':
            break
        else:
            print(f'Unrecognized input={u_input}, please try again\r\n')
            
    


if __name__ == '__main__':
    print('Starting middleware process...')
    run_middleware()