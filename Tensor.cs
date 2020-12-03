using System.Collections.Generic;
using System.Linq;
using System;

namespace Test
{
    public class Tensor<T>
    {
        private T[] _source;
        private int[] _dimensions;
        private int[] _dimensionsDegrees;

        public int DimensionsCount => _dimensions.Length;

        public int GetLength(int dimIndex) => _dimensions[dimIndex];

        public Tensor(params int[] dimensions)
        {
            if (dimensions.Any(d => d < 0))
                throw new ArgumentException("Invalid dimesion");

            _dimensions = dimensions;
            _source = new T[dimensions.Aggregate((d1, d2) => d1 * d2)];

            InitializeDegrees();
        }

        public Tensor(int[] dimensions, T[] source)
        {
            if (dimensions.Any(d => d < 0))
                throw new ArgumentException("Invalid dimesion");

            if (source.Length != dimensions.Aggregate((d1, d2) => d1 * d2))
                throw new ArgumentException("Invalid source length");

            _source = source;
            _dimensions = dimensions;

            InitializeDegrees();
        }

        public T GetItem(params int[] indexes)
        {
            int index = GetIndex(indexes);
            return _source[index];
        }

        public void SetItem(T value, params int[] indexes)
        {
            int index = GetIndex(indexes);
            _source[index] = value;
        }

        private int GetIndex(params int[] indexes)
        {
            if (indexes.Length != _dimensions.Length)
                throw new InvalidOperationException("Invalid indexes count");

            int index = 0;
            for (int i = 0; i < _dimensions.Length; i++)
            {
                index += indexes[i] * _dimensionsDegrees[i];
            }

            return index;
        }

        private void InitializeDegrees()
        {
            _dimensionsDegrees = new int[_dimensions.Length];
            _dimensionsDegrees[0] = 1;

            for (int i = 1; i < _dimensionsDegrees.Length; i++)
            {
                _dimensionsDegrees[i] = _dimensionsDegrees[i - 1] * _dimensions[i - 1];
            }
        }

        public T this[params int[] indexes]
        {
            get { return GetItem(indexes); }
            set { SetItem(value, indexes); }
        }

        public Tensor<T> GetCrop(int[] startPoint, int[] endPoint)
        {
            if (startPoint.Length != endPoint.Length || startPoint.Length != _dimensions.Length)
                throw new ArgumentException("Invalid boundaries");

            for (int i = 0; i < startPoint.Length; i++)
            {
                if (startPoint[i] > endPoint[i])
                {
                    var temp = startPoint;
                    startPoint = endPoint;
                    endPoint = temp;
                    break;
                }

                if (endPoint[i] > startPoint[i]) break;
            }

            var dimensions = Enumerable.Range(0, startPoint.Length)
                .Select(i => Math.Abs(endPoint[i] - startPoint[i]) + 1)
                .Where(i => i != 0)
                .ToArray();

            var source = GetAllIndexesBetweenPoints(startPoint, endPoint)
                .Reverse()
                .Select(i => GetItem(i))
                .ToArray();

            return new Tensor<T>(dimensions, source);
        }


        public IEnumerable<int[]> GetAllIndexesBetweenPoints(int[] startPoint, int[] endPoint)
        {
            var stack = new Stack<Tuple<int[], int>>();
            stack.Push(Tuple.Create(startPoint, 0));

            while (stack.Count != 0)
            {
                var current = stack.Pop();
                var indexes = current.Item1;
                var dimensionIndex = current.Item2;

                if (dimensionIndex != startPoint.Length)
                {
                    int delta = endPoint[dimensionIndex] - startPoint[dimensionIndex];
                    int count = Math.Abs(delta);
                    int dirrection = delta / (count == 0 ? 1 : count);

                    var newIndexes = new int[startPoint.Length];
                    indexes.CopyTo(newIndexes, 0);
                    stack.Push(Tuple.Create(newIndexes, dimensionIndex + 1));

                    for (int i = 0; i < count; i++)
                    {
                        newIndexes = new int[startPoint.Length];
                        indexes.CopyTo(newIndexes, 0);
                        newIndexes[dimensionIndex] += (i + 1) * dirrection;
                        stack.Push(Tuple.Create(newIndexes, dimensionIndex + 1));
                    }
                }
                else
                {
                    yield return indexes;
                }
            }
        }
    }
}