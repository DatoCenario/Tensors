using System;
using System.Linq;
using System.Collections.Generic;

namespace Tensors
{
    public static class TensorPrintExtensions
    {
        public static void PrintTensorToConsole<T>(this Tensor<T> tensor)
        {
            var dimensionDesc = tensor.Dimensions
                .Select(d => d.ToString())
                .Aggregate((d1,d2) => $"{d1}x{d2}");

            Console.WriteLine($"Tensor {dimensionDesc}");
            if(tensor.DimensionsCount == 1) PrintOneDimensionalTensor(tensor);
            else
            {
                PrintNDimensionalTensor(tensor, tensor.DimensionsCount - 1, Enumerable.Empty<int>());
                Console.WriteLine();
            }
        }

        private static void PrintOneDimensionalTensor<T>(Tensor<T> tensor)
        {
            for (int i = 0; i < tensor.GetLength(0); i++)
            {
                Console.Write($"{tensor[i]} ");
            }

            Console.WriteLine();
        }

        private static void PrintNDimensionalTensor<T>(Tensor<T> tensor, int dimIndex,
            IEnumerable<int> otherIndexes)
        {
            if (dimIndex == 1) PrintTwoDimensionalCut(tensor, otherIndexes);
            else
            {
                var indent = new string(' ', (tensor.DimensionsCount - dimIndex) * 2);
                Console.WriteLine($"{indent}{dimIndex + 1} dimension cut begin\n");
                for (int i = 0; i < tensor.GetLength(dimIndex); i++)
                {
                    PrintNDimensionalTensor(tensor, dimIndex - 1, new int[] { i }.Concat(otherIndexes));
                }
                Console.WriteLine($"{indent}{dimIndex + 1} dimension cut end\n");
            }
        }

        private static void PrintTwoDimensionalCut<T>(Tensor<T> tensor, IEnumerable<int> otherInexes)
        {
            var indexes = new int[] { 0, 0 }.Concat(otherInexes).ToArray();
            var indent = new string(' ', (indexes.Length - 1) *  2);

            Console.WriteLine($"{indent}2 dimension cut begin");
            Console.WriteLine($"{indent}#####################");

            for (indexes[1] = 0; indexes[1] < tensor.GetLength(1); indexes[1]++)
            {
                Console.Write($"{indent}");
                for (indexes[0] = 0; indexes[0] < tensor.GetLength(0); indexes[0]++)
                {
                    Console.Write($"{tensor.GetItem(indexes)} ");
                }

                Console.WriteLine();
            }

            Console.WriteLine($"{indent}#####################");
            Console.WriteLine($"{indent}2 dimension cut end\n");
        }
    }
}