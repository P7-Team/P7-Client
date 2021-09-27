import socket
from protocol import encode_msg, decode_msg, MSG_TYPE_CONNECT, MSG_TYPE_MESSAGE

LOCAL_PORT = 55667

SERVER_PORT = 44556
SERVER_IP = '127.0.0.1'

REMOTE_IP = '127.0.0.1'



def run()->None:
    remote_conn = connect()
    ping_pong(remote_conn)
    pass

def connect()->socket.socket:
    print(f'Creating TCP connection to server at IP={SERVER_IP}, PORT={SERVER_PORT}')
    # Create the server connectoin
    server_conn = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server_conn.connect((SERVER_IP, SERVER_PORT))

    # Send initial connect message
    print(f'Sending connect message to SERVER with requested recepient IP={REMOTE_IP}, PORT={REMOTE_PORT}, MSG_TYPE={MSG_TYPE_CONNECT}')
    connect_msg:bytes = encode_msg(MSG_TYPE_CONNECT, REMOTE_IP, REMOTE_PORT)
    server_conn.send(connect_msg)

    return server_conn

def ping_pong(conn:socket.socket)->None:
    try:
        while True:
            # Start by sending a message
            msg:bytes = create_content_message()
            conn.send(msg)
            
            # Receive message from connection
            data:bytes = conn.recv(1024)
            if data is None:
                continue
            decoded_msg:tuple = decode_msg(data)
            print(f'Received messsage: {decoded_msg}')

    except KeyboardInterrupt:
        print('PingPong was interrupted by user, closing connection...')
        conn.close()


def create_content_message():
    randi_msg:str = 'This here is the content'#str(random.randrange(1001, 111000))
    print(f'Creating message with content={randi_msg}, msg_type={MSG_TYPE_MESSAGE}')
    msg:bytes = encode_msg(MSG_TYPE_MESSAGE, REMOTE_IP, REMOTE_PORT, randi_msg)
    return msg

if __name__ == '__main__':
    if True:
        temp = REMOTE_PORT
        REMOTE_PORT = LOCAL_PORT
        LOCAL_PORT = temp
    print('Starting message PingPong...')
    run()
    print('Finished message PingPong...')
