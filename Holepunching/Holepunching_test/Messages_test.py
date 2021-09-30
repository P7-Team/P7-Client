from Holepunching import Messages


def test_address_to_message():
    assert Messages.address_to_message(("192.168.10.0", 5000)) == b'192.168.10.0:5000'


