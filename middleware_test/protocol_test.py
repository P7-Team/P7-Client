import unittest
from protocol import SEP_ADDR, SEP_CONTENT, get_msg_len, MSG_TYPE_CONNECT, extract_msg, MSG_ENCAPSULATION_S, MSG_ENCAPSULATION_E, MSG_ENCODING, encapsulate_msg, add_len_header, encode_msg, decode_msg

class TestProtocol(unittest.TestCase):
    
    def test_msg_len(self):
        # Setup
        conn_id:str = 'Connection_id'
        msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id
        final_msg:str = str(len(msg)) + '(' + msg + ')'
        msg_data:bytes = final_msg.encode('utf-8')

        # Act
        actual:int = get_msg_len(msg_data)

        # Assert
        expected:int = len(msg)
        self.assertEqual(actual, expected)
    
    def test_extract_msg(self):
        # Setup
        conn_id:str = 'Connection_id'
        msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id
        encapsulated_msg = MSG_ENCAPSULATION_S + msg + MSG_ENCAPSULATION_E
        full_msg:str = str(len(encapsulated_msg)) + encapsulated_msg
        encoded_msg:bytes = full_msg.encode(MSG_ENCODING)

        expected:str = msg.encode(MSG_ENCODING)

        # Act
        actual:bytes = extract_msg(encoded_msg)

        # Assert
        self.assertEqual(expected, actual)

    def test_encapsulate_msg(self):
        # Setup
        conn_id:str = 'Connection_id'
        raw_msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id

        expected:str = MSG_ENCAPSULATION_S + raw_msg + MSG_ENCAPSULATION_E
        # Act
        actual:str = encapsulate_msg(raw_msg) 

        # Assert
        self.assertEqual(expected, actual)

    def test_add_len_header(self):
        # Setup
        conn_id:str = 'Connection_id'
        raw_msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id
        encap_msg:str = MSG_ENCAPSULATION_S + raw_msg + MSG_ENCAPSULATION_E

        expected:str = str(len(encap_msg)) + encap_msg
        # Act
        actual:str = add_len_header(encap_msg)

        # Assert
        self.assertEqual(expected, actual)

    def test_encode_msg(self):
        # Setup
        conn_id:str = 'Connection_id'
        raw_msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id
        encapsulated_msg:str = MSG_ENCAPSULATION_S + raw_msg + MSG_ENCAPSULATION_E
        expected_msg:str = str(len(encapsulated_msg)) + encapsulated_msg

        expected:bytes = expected_msg.encode(MSG_ENCODING)

        # Act
        actual:bytes = encode_msg(MSG_TYPE_CONNECT, conn_id)

        # Assert
        self.assertEqual(expected, actual)
    
    def test_decode_msg(self):
        # Setup
        conn_id:str = 'Connection_id'
        raw_msg:str = MSG_TYPE_CONNECT + SEP_ADDR + conn_id
        encoded_msg:bytes = raw_msg.encode(MSG_ENCODING)

        expected:str = (MSG_TYPE_CONNECT, conn_id, '')

        # Act
        actual:bytes = decode_msg(encoded_msg)

        # Assert
        self.assertEqual(expected, actual)


if __name__ == '__main__':
    unittest.main()