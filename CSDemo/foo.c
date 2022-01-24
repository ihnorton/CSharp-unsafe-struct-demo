#include <stdlib.h>
#include <stdio.h>

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