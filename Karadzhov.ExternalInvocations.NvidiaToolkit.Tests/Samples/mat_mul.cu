#include "cuda_runtime.h"
#include "cublas_v2.h"

extern "C" __declspec(dllexport) void __cdecl matrixMul(int m, int n, int k, float* a, float* b, float* c);

__global__ void devMatrivMultiply(int m, int n, int k, float* d_a, float* d_b, float* d_c)
{
	cublasHandle_t cnpHandle;
	cublasCreate(&cnpHandle);

	float* params = (float*)malloc(2*sizeof(float));
	params[0] = 1.0f;
	params[1] = 0.0f;

	cublasSgemm(cnpHandle,
				CUBLAS_OP_N, CUBLAS_OP_N,
				m, n, k,
				&params[0],
				d_a, m,
				d_b, k,
				&params[1],
				d_c, m);

	cublasDestroy(cnpHandle);

	free(params);
}

void __cdecl matrixMul(int m, int n, int k, float* a, float* b, float* c)
{
	float *d_a, *d_b, *d_c;

	cudaMalloc((void **)&d_a, m * k * sizeof(float));
	cudaMalloc((void **)&d_b, k * n * sizeof(float));
	cudaMalloc((void **)&d_c, m * n * sizeof(float));

	cublasSetVector(m * k, sizeof(float), a, 1, d_a, 1);
	cublasSetVector(k * n, sizeof(float), b, 1, d_b, 1);
	cublasSetVector(m * n, sizeof(float), c, 1, d_c, 1);

	devMatrivMultiply<<<1, 1>>>(m, n, k, d_a, d_b, d_c);

	cublasGetVector(m * n, sizeof(float), d_c, 1, c, 1);

	cudaFree(d_a);
	cudaFree(d_b);
	cudaFree(d_c);
}