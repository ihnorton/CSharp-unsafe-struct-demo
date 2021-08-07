using Libdl

h = Libdl.dlopen("build/libaux.dylib")
f = Libdl.dlsym(h, :aux_sum_double)

v = Array{Float64}([1,2,3,4])
out = Ref{Float64}(0)

res = ccall(f, Int32, (Ptr{Float64}, UInt64, Ptr{Float64}), v, sizeof(v), out)

@assert(res == 0)
@assert(out[] == sum(v))

println("Done!")
