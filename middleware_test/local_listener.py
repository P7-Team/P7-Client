import socket
import selectors
from ping_pong import SERVER_IP, SERVER_PORT
from protocol import decode_msg, MSG_TYPE_CONNECT, MSG_TYPE_MESSAGE, extract_msg, re_encode_msg
from queue import Queue
from tcp_util import recv_next_msg, is_message_on_stream

sel = selectors.DefaultSelector()

addr_to_conn:dict = {}
conn_to_addr:dict = {}
connections:dict = {}
message_queues:dict = {}

def start_listening()->None:
    s: socket.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.bind((SERVER_IP, SERVER_PORT))
    s.listen(100)
    sel.register(s, selectors.EVENT_READ, accept_new_connection)

def accept_new_connection(socket:socket.socket, mask)->None:
    conn, addr = socket.accept()
    print(f'Accepted connection=<{conn}>, from=<{addr}>')
    sel.register(conn, selectors.EVENT_READ, pair_connection)

    # Store the address information for later use
    addr_to_conn[addr] = conn
    conn_to_addr[conn] = addr


def pair_connection(conn:socket.socket, mask)->None:
    while is_message_on_stream(conn):
        data:bytes = recv_next_msg(conn)
        extracted_msg:bytes = extract_msg(data)
        msg_type, id, content = decode_msg(extracted_msg)
        source_addr = conn_to_addr[conn]

        # Handle based on message type
        if msg_type == MSG_TYPE_CONNECT:
            handle_connect(source_addr, id)


        elif msg_type == MSG_TYPE_MESSAGE:
            handle_message(source_addr, id, data)

def send_queued_messages(connection_id)->None:
    recipient_list:list = connections[connection_id]

    if len(recipient_list) > 1:
        if connection_id in message_queues.keys():
            queue:Queue = message_queues[connection_id]
            while not queue.empty():
                msg, sender_addr = queue.get()
                for addr in recipient_list:
                    if addr != sender_addr:
                        print(f'Sending queued message <{msg}> from <{sender_addr}> to <{addr}>')
                        conn:socket.socket = addr_to_conn[addr]
                        conn.send(re_encode_msg(msg))
            # All in queue have been sent, remember to pop
            message_queues.pop(connection_id)

def handle_connect(from_addr, connection_id)->None:
    # Check if connection_id already exists
    if connection_id not in connections.keys():
        connections[connection_id] = list()

    # Add the address to the list on the connection
    connection:list = connections[connection_id]
    connection.append(from_addr)

    print(f'Added addr <{from_addr}> to connection <{connection}>')
    print(f'Full list of connections are now <{connections}>')

    print(f'Handled connect message from addr={from_addr} on ID={connection_id}')
    # Send the messages that have been queued from the remote side
    send_queued_messages(connection_id)

def handle_message(sender_addr, connection_id:str, msg:bytes)->None:
    recipient_list:list = connections[connection_id]

    print(f'Handle message initiated by sender_addr=<{sender_addr}> for connection_id=<{connection_id}>')

    # There are other connected besides you
    if len(recipient_list) > 1:
        for addr in recipient_list:
            if addr != sender_addr:
                print(f'\tSending message <{msg}> from <{sender_addr}> to <{addr}>')
                conn:socket.socket = addr_to_conn[addr]
                conn.send(re_encode_msg(msg))

    else:
        # Check if queue exists
        if connection_id not in message_queues.keys():
            message_queues[connection_id] = Queue(maxsize=0)
        
        print(f'Not enough connected to ID=<{connection_id}> from sender_addr=<{sender_addr}>')
        queue:Queue = message_queues[connection_id]
        queue.put((msg, sender_addr))

if __name__ == '__main__':
    print(f'Starting local listener on IP=<{SERVER_IP}>, PORT=<{SERVER_PORT}>')
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