MSG_TYPE_CONNECT = 'CONNECT'
MSG_TYPE_MESSAGE = 'MESSAGE'

# Message: msg_type:id<!split#>content
SEP_ADDR = ':'
SEP_CONTENT = '<!split#>'

MSG_ENCODING = 'utf-8'
MSG_ENCAPSULATION_S = '('
MSG_ENCAPSULATION_E = ')'

# Message with length: msg_len(msg_type:id<!split#>content)

def extract_msg(data:bytes)->bytes:
    data_decoded:str = data.decode(MSG_ENCODING).strip()

    print(f'Extracting the msg of the decoded data={data_decoded}')
    # Remove the msg_len and the msg_encapsulation
    msg_start:int = data_decoded.find(MSG_ENCAPSULATION_S) + 1
    msg_end:int = data_decoded.find(MSG_ENCAPSULATION_E)
    msg:str = data_decoded[msg_start:msg_end]
    print(f'Extracted msg is: {msg}')
    return msg.encode(MSG_ENCODING)


def decode_msg(data:bytes)->tuple:
    type, id = data.decode(MSG_ENCODING).strip().split(SEP_ADDR)
    content = ''
    if SEP_CONTENT in id:
        id, content = id.split(SEP_CONTENT)
    return (type, id, content)

def get_msg_len(data:bytes)->int:
    data_str:str = data.decode(MSG_ENCODING).strip()
    msg_len_s_idx = 0
    msg_len_e_idx = data_str.find('(')
    return int(data_str[msg_len_s_idx:msg_len_e_idx])

def encode_msg(msg_type:str, id:str, content:str=None)->bytes:
    msg_str:str = f'{msg_type}:{id}' + ('<!split#>' + content if content is not None else '')

    return package_msg(msg_str)

def re_encode_msg(msg:bytes)->bytes:
    raw_msg:str = extract_msg(msg).decode(MSG_ENCODING)

    return package_msg(raw_msg)

def package_msg(raw_msg:str)->bytes:
    encapsulated_msg:str = encapsulate_msg(raw_msg)
    msg_with_header:str = add_len_header(encapsulated_msg)
    return msg_with_header.encode(MSG_ENCODING)

def encapsulate_msg(msg:str):
    return MSG_ENCAPSULATION_S + msg + MSG_ENCAPSULATION_E

def add_len_header(encapsulated_msg:str):
    msg_with_header:str = str(len(encapsulated_msg)) + encapsulated_msg
    return msg_with_header