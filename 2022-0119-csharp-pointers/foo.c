#include <stdlib.h>
#include <stdio.h>
#include <string.h>

typedef struct handle_t {
  int internal;
} handle_t;

void handle_alloc(handle_t **p) {
  handle_t* tmp;
  tmp = (handle_t*)malloc(sizeof(handle_t));

  printf("handle_t* is: %p\n", tmp);
  *p = tmp;
}

void doit(int in, int *out) { *out = in + 10; }

void return_string(const char** out) {
  printf("got char**: %p\n", out);

  const char msg[] = "hello, world";
  char* p = (char*)malloc(sizeof(msg) + 1);
  p[sizeof(msg)] = '\0';
  strcpy(p, (char*)&msg);

  printf("char* will be: %p\n", p);
  *out = p;

  printf("char* should be: %p\n", *out);
}

void return_int32_array(void** out, uint32_t* size) {
  uint32_t nelem = 3;
  int32_t data[] = {1000,22,3};

  // make some output space
  void* p = malloc(sizeof(data));

  // copy the local to temp pointer
  memcpy(p, &data, sizeof(data));

  *out = p;
  printf("void* out will be: %p\n", p);

  *size = sizeof(data);
  printf("size* out will be: %p\n", *size);

  printf("!! done\n");
}