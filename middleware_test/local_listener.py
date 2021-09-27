import socket
import selectors
from ping_pong import SERVER_IP, SERVER_PORT
from protocol import decode_msg, MSG_TYPE_CONNECT, MSG_TYPE_MESSAGE
from queue import Queue

established_connections: dict = {}
waiting_connections: dict = {}
conn_to_addr: dict = {}
addr_to_conn: dict = {}
message_queues: dict = {}
port_map: dict = {}

sel = selectors.DefaultSelector()

def start_listening()->None:
    s: socket.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((SERVER_IP, SERVER_PORT))
    s.listen(100)
    #s.setblocking(False) - This seems to break everything
    sel.register(s, selectors.EVENT_READ, accept_new_connection)

def accept_new_connection(socket:socket.socket, mask)->None:
    conn, addr = socket.accept()
    print(f'Accepted connection={conn}, from={addr}')
    #conn.setblocking(False) - This seems to break everything
    sel.register(conn, selectors.EVENT_READ, pair_connection)

    # Store the address information for later use

    if conn not in conn_to_addr.keys():
        conn_to_addr[conn] = addr
        addr_to_conn[addr] = conn

def pair_connection(conn:socket.socket, mask)->None:
    data:bytes = conn.recv(1024)
    msg_type, ip, port, content = decode_msg(data)
    addr = conn_to_addr[conn]

    # Handle based on message type
    if msg_type == MSG_TYPE_CONNECT:
        handle_connect(addr, ip, port)


    elif msg_type == MSG_TYPE_MESSAGE:
        handle_message(addr, ip, port, data)


def send_queued_messages(from_addr, to_addr)->None:
    # Check if there are a queue for the sender
    if to_addr not in message_queues.keys():
        return
    
    queue:Queue = message_queues[to_addr]
    to_conn: socket.socket = addr_to_conn[to_addr]
    while queue.not_empty:
        queued_msg:bytes = queue.get()
        to_conn.send(queued_msg)



def handle_connect(addr, remote_ip, remote_port)->None:
    print(f'Checking local addr={addr} to remote information={(remote_ip, remote_port)}')
    if addr in established_connections.keys() or \
        (remote_ip, remote_port) not in waiting_connections.keys():
        return
    
    # Set both ways as established
    established_connections[addr] = (remote_ip, remote_port)
    established_connections[(remote_ip, remote_port)] = addr
    # Remove connections from waiting
    waiting_connections.pop(addr)
    waiting_connections.pop((remote_ip, remote_port))

    # Send the messages that have been queued from the remote side
    send_queued_messages(addr, (remote_ip, remote_port))

def handle_message(sender_addr, destination_ip:str, destination_port:int, msg:bytes)->None:
    if (destination_ip, destination_port) in established_connections.keys():
        print(f'Connection exists, sending message to={(destination_ip, destination_port)}')
        destination_conn:socket.socket = addr_to_conn[(destination_ip, destination_port)]
        destination_conn.send(msg)
        return


    if (destination_ip, destination_port) not in message_queues.keys(): # Create a new queue and queue new message
        print(f'Stuff added for queue belonging to={(destination_ip, destination_port)}')
        queue: Queue = Queue(maxsize=0)
        queue.put(msg)
        message_queues[(destination_ip, destination_port)] = queue
    else: # Get existing queue and queue new mssage
        print(f'Stuff added for queue belonging to={(destination_ip, destination_port)}')
        queue: Queue = message_queues[(destination_ip, destination_port)]
        queue.put(msg)


if __name__ == '__main__':
    print(f'Starting local listener on IP={SERVER_IP}, PORT={SERVER_PORT}')
    try:
        start_listening()

        while True:
            events = sel.select()
            for key, mask in events:
                callback = key.data
                callback(key.fileobj, mask)
    except KeyboardInterrupt:
        print('Local Listener interrupted by User...')
        sel.close()