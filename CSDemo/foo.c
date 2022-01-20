typedef struct handle_t {
  int internal;
} handle_t;

void handle_alloc(handle_t **p) { *p = malloc(sizeof(handle_t)); }

void doit(int in, int *out) { *out = in + 10; }
