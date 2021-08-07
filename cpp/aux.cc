#include "aux.h"

#include <iostream>
#include <vector>
#include <span>

double sum(std::span<double> v) {
    double res = 0;
    for (auto& x : v) {
        res += x;
    }
    return res;
}

CStatus aux_sum_double(double* inptr, size_t nelem, double* outptr) {
    auto v = std::span<double>(inptr, nelem);
    double result = sum(v);

    *outptr = result;

    return CStatus::Ok;
}