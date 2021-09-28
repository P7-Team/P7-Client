import socket
from protocol import get_msg_len

def recv_next_msg(conn: socket.socket)->bytes:
    print(f'<recv_next_msg>: I am going to do a blocking peek...')
    peeked_data:bytes = conn.recv(1024, socket.MSG_PEEK)
    if peeked_data is None or peeked_data == '':
        return None
    
    print(f'Received peeked_data={peeked_data}')

    msg_len:int = get_msg_len(peeked_data)
    len_len:int = len(str(msg_len))
    print(f'The length of the first message is: {msg_len} and the length of the msg_length is {len_len}')
    conn.recv(len_len)

    received_data:bytes = conn.recv(msg_len)
    print(f'The message itself retrieved is: {received_data}')

    try:
        conn.setblocking(False)
        rest_of_buffer: bytes = conn.recv(1024, socket.MSG_PEEK)
        print(f'Checking the content of the buffer: <{rest_of_buffer}>')
    except BlockingIOError:
        print('Buffer is currently empty...')
    finally:
        conn.setblocking(True)

    return received_data

def is_message_on_stream(conn: socket.socket)->bool:
    ret_val:bool = True
    try:
        conn.setblocking(False)
        peeked_data:bytes = conn.recv(1024, socket.MSG_PEEK)
        ret_val = True
    except BlockingIOError:
        ret_val = False
    finally:
        conn.setblocking(True)
    
    return ret_val
