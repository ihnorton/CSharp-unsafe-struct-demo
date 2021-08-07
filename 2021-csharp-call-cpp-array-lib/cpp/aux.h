#include "stddef.h"

extern "C" {

enum CStatus {
    Ok = 0,
    Fail = -1
};

CStatus aux_sum_double(double* inptr, size_t nelem, double* outptr);

}