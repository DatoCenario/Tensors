using System;
using System.Linq;

namespace Tensors
{
    class Program
    {
        static Random Random = new Random();
        static void Main(string[] args)
        {
            //var tensor = new Tensor<int>(10,10,2);
            //tensor.PrintTensorToConsole();
            Test2();
        }

        public static void Test2()
        {
            var values = Enumerable.Range(0, 125)
                .Select(v => Random.Next(0, 9))
                .ToArray();

            var tensor = new Tensor<int>(new int[] { 5, 5, 5 }, values);
            var crop = tensor.GetCrop(new int[] { 3, 3, 3 }, new int[] { 3, 1, 1 });
            
            tensor.PrintTensorToConsole();
            crop.PrintTensorToConsole();
        }
        public static void Test1()
        {
            var values = Enumerable.Range(0, 25)
                .Select(v => Random.Next(0, 9))
                .ToArray();

            var tensor = new Tensor<int>(new int[] { 5, 5 }, values);
            var crop = tensor.GetCrop(new int[] { 3, 1 }, new int[] { 3, 3 });

            tensor.PrintTensorToConsole();
            crop.PrintTensorToConsole();
        }
    }
}
