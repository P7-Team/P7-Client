
MSG_TYPE_CONNECT = 'CONNECT'
MSG_TYPE_MESSAGE = 'MESSAGE'

# Message: msg_type:ip:port<!split#>content
SEP_ADDR = ':'
SEP_CONTENT = '<!split#>'

def decode_msg(data:bytes)->tuple:
    datas = data.decode('utf-8').strip()
    print(f'Received message={datas}')
    type, ip, port = data.decode('utf-8').strip().split(SEP_ADDR)
    content = ''
    if SEP_CONTENT in port:
        port, content = port.split(SEP_CONTENT)
    return (type, ip, int(port), content)

def encode_msg(msg_type:str, ip:str, port:int, content:str=None)->bytes:
    msg_str:str = f'{msg_type}:{ip}:{port}' + ('<!split#>' + content if content is not None else '')
    print(f'msg pre-encode={msg_str}')
    return msg_str.encode('utf-8')

