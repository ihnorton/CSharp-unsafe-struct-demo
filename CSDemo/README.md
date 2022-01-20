# Demo using unsafe struct for opaque pointer handles

Technique from: https://www.codeproject.com/Articles/339290/PInvoke-pointer-safety-Replacing-IntPtr-with-unsaf

The expected output here is that the C level allocation matches what C# sees on return:

```
gcc foo.c -o libfoo.dylib -shared
result: 12
handle_t* is: 0x7fea8e81f210
C# handle_t* is: 7FEA8E81F210
```
