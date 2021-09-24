import socket
import time

IP = '127.0.0.1'
PORT = 1234



def run_heartbeat():
    print(f'Starting heartbeat connecting to IP={IP} and PORT={PORT}')
    with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
        s.connect((IP, PORT))

        while True:
            send_hearbeat(s)
            time.sleep(5)

def send_hearbeat(conn: socket.socket):
    print('Sending heartbeat')
    conn.sendall(b'Heartbeat')

if __name__ == '__main__':
    run_heartbeat()
