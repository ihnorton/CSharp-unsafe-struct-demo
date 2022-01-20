#include <stdlib.h>
#include <stdio.h>

typedef struct handle_t {
  int internal;
} handle_t;

void handle_alloc(handle_t **p) {
  *p = malloc(sizeof(handle_t));

  printf("handle_t* is: %p\n", *p);
}

void doit(int in, int *out) { *out = in + 10; }
