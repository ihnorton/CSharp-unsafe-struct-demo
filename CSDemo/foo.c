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
  const char msg[] = "hello, world";
  char* p = (char*)malloc(sizeof(msg) + 1);
  strcpy(p, (char*)&msg);

  printf("char* will be: %p\n", p);
  *out = p;
}