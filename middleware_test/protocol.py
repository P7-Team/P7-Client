
MSG_TYPE_CONNECT = 'CONNECT'
MSG_TYPE_MESSAGE = 'MESSAGE'

# Message: msg_type:id<!split#>content
SEP_ADDR = ':'
SEP_CONTENT = '<!split#>'

def decode_msg(data:bytes)->tuple:
    datas:str = data.decode('utf-8').strip()
    print(f'Pre-decoded message={datas}')
    type, id = data.decode('utf-8').strip().split(SEP_ADDR)
    content = ''
    if SEP_CONTENT in id:
        id, content = id.split(SEP_CONTENT)
    return (type, id, content)

def encode_msg(msg_type:str, id:str, content:str=None)->bytes:
    msg_str:str = f'{msg_type}:{id}' + ('<!split#>' + content if content is not None else '')
    print(f'msg pre-encode={msg_str}')
    return msg_str.encode('utf-8')

