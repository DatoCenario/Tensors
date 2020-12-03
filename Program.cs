using System;

namespace Test
{
    class Program
    {   
        static Random Random = new Random();
        static void Main(string[] args)
        {
            var tensor = new Tensor<int>(5,5,5,5,5);
            var crop = tensor.GetCrop(new int[] {1,1,0,0,0}, new int[] {5,5,0,0,0});
            
        }
    }
}
