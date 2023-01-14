import numpy as np

with open("test.bin", "wb") as f:
    f.write(np.array(1, dtype=np.int32))
    f.write(np.array(2.345).tobytes())
    s = b"abcdef\0"
    f.write(np.array(len(s), dtype=np.uint32).tobytes())
    f.write(s)
