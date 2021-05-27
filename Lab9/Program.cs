using System;
using System.Collections.Generic;

namespace Matrix
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input rank of matrix: ");
            var rank = Convert.ToInt32(Console.ReadLine());

            double[][] elements = new double[rank][];
            for (int i = 0; i < rank; i++)
                elements[i] = new double[rank];

            Console.WriteLine("Inputing elements...");
            for (int i = 0; i < rank; i++)
            {
                Console.WriteLine("Input {0} row", i + 1);
                var tempValues = Console.ReadLine().Split(' ');
                for (int j = 0; j < rank; j++)
                    elements[i][j] = Convert.ToDouble(tempValues[j]);
            }

            Matrix matrix = new Matrix(elements);

            var ret = matrix.IterationsMethod();

            Console.WriteLine("Eigenvalue: " + ret.Item1);
            Console.Write("Eigenvector: ");
            foreach (var val in ret.Item2)
            {
                Console.Write(val + " ");
            }

            /*var temp = matrix.DanilevskyMethod();
            foreach (var val in temp.Item1)
            {
                Console.WriteLine(val + " ");
            }
            double[] arr = { 0.7796, 0.4448, -1.7443, -4.48015 };

            var y = new List<double[]>();
            foreach (var val in arr)
            {
                var yTemp = new double[4];
                double count = 1;
                for (int i = 0; i < 4; i++)
                {
                    yTemp[3 - i] = count;
                    Console.Write(count + " ");
                    count *= val;
                }
                y.Add(yTemp);
                Console.WriteLine();
            }
            foreach(var val in y)
            {
                var X = temp.Item2 * val;
                foreach (var val2 in X)
                {
                    Console.WriteLine(val2 + " ");
                }
                Console.WriteLine();
            }*/



        }
    }
}
