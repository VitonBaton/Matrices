using System;
using System.Linq;

namespace Matrix
{
    class Matrix
    {
        double[][] elements;
        int rank;
        public Matrix(double[][] elements)
        {
            this.elements = elements.CopyThis();
            this.rank = elements.Length;
        }

        public double[] this[int i]
        {
            get { return elements[i]; }
            set { elements[i] = value; }
        }

        public double Determinator()
        {
            double[][] tempMatrix = elements.CopyThis();

            // k - current column/row 
            for (int k = 0; k < rank; k++)
            {
                // j - current row
                for (int j = k + 1; j < rank; j++)
                {
                    // div all elements in this row by key element
                    var coeff = tempMatrix[j][k] / tempMatrix[k][k];
                    for (int i = k; i < rank; i++)
                    {
                        tempMatrix[j][i] -= coeff * tempMatrix[k][i];
                    }
                }

                for (int j = 0; j < k; j++)
                {
                    var coeff = tempMatrix[j][k] / tempMatrix[k][k];
                    for (int i = k; i < rank; i++)
                    {
                        tempMatrix[j][i] -= coeff * tempMatrix[k][i];
                    }
                }
            }
            Console.WriteLine();
            double det = 1;
            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < rank; j++)
                    Console.Write("{0} ", tempMatrix[i][j]);
                Console.WriteLine();
            }
            Console.WriteLine();
            for (int i = 0; i < rank; i++)
                det *= tempMatrix[i][i];
            return det;
        }

        public Matrix Inverse()
        {
            if (Math.Abs(Determinator()) < 0.000001)
                return null;

            double[][] inversed = new double[rank][];
            for (int i = 0; i < rank; i++)
            {
                inversed[i] = new double[rank];
                inversed[i][i] = 1;
            }


            var tempMatrix = elements.CopyThis();
            // k - current column/row 
            for (int k = 0; k < rank; k++)
            {
                for (int j = k + 1; j < rank; j++)
                {
                    var coeff = tempMatrix[j][k] / tempMatrix[k][k];
                    for (int i = k; i < rank; i++)
                    {
                        tempMatrix[j][i] -= coeff * tempMatrix[k][i];

                    }
                    for (int i = 0; i < rank; i++)
                        inversed[j][i] -= coeff * inversed[k][i];
                }

                for (int j = 0; j < k; j++)
                {
                    var coeff = tempMatrix[j][k] / tempMatrix[k][k];
                    for (int i = k; i < rank; i++)
                    {
                        tempMatrix[j][i] -= coeff * tempMatrix[k][i];
                    }
                    for (int i = 0; i < rank; i++)
                        inversed[j][i] -= coeff * inversed[k][i];
                }
            }

            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < rank; j++)
                    inversed[i][j] /= tempMatrix[i][i];
            }


            return new Matrix(inversed);
        }

        public override string ToString()
        {
            var retValue = "";

            for (int i = 0; i < rank; i++)
            {
                for (int j = 0; j < rank; j++)
                    retValue += elements[i][j] + " ";
                retValue += "\n";
            }
            return retValue;
        }

        public static Matrix operator *(Matrix m1, Matrix m2)
        {
            if (m1.rank != m2.rank)
            {
                throw new ArgumentException("Different ranks");
            }
            double[][] ret = new double[m1.rank][];
            for (int i = 0; i < m1.rank; i++)
            {
                ret[i] = new double[m1.rank];
            }

            for (int i = 0; i < m1.rank; i++)
            {
                for (int j = 0; j < m1.rank; j++)
                {
                    ret[i][j] = 0;
                    for (int k = 0; k < m1.rank; k++)
                    {
                        ret[i][j] += m1.elements[i][k] * m2.elements[k][j];
                    }
                }
            }
            return new Matrix(ret);
        }

        public static double[] operator *(Matrix m1, double[] m2)
        {
            if (m1.rank != m2.Count())
            {
                throw new ArgumentException("Different ranks");
            }
            double[] ret = new double[m1.rank];

            for (int i = 0; i < m1.rank; i++)
            {
                ret[i] = 0;
                for (int k = 0; k < m1.rank; k++)
                {
                    ret[i] += m1.elements[i][k] * m2[k];
                }
            }
            return ret;
        }

        public (double[], Matrix) DanilevskyMethod()
        {
            double[][] sim = new double[rank][];
            for (int k = 0; k < rank; k++)
            {
                sim[k] = new double[rank];
                sim[k][k] = 1;
            }
            var similarity = new Matrix(sim);
            var a = new Matrix(this.elements.CopyThis());
            for (int i = rank - 2; i >= 0; i--)
            {
                double[][] tempM = new double[rank][];
                double[][] tempMInversed = new double[rank][];
                for (int k = 0; k < rank; k++)
                {
                    tempMInversed[k] = new double[rank];
                    tempM[k] = new double[rank];
                    tempM[k][k] = 1;
                    tempMInversed[k][k] = 1;
                }
                var m = new Matrix(tempM);
                var mInversed = new Matrix(tempMInversed);
                var temp = a[i + 1][i];
                var ind = i + 1;
                for (int k = 0; k < rank; k++)
                {
                    if (i == k)
                    {
                        m[i][k] = 1 / temp;
                        mInversed[i][k] = a[ind][k];
                        continue;
                    }
                    m[i][k] = -a[ind][k] / temp;
                    mInversed[i][k] = a[ind][k];
                }
                similarity = similarity * m;
                a = mInversed * a;
                a = a * m;
            }
            return (a[0], similarity);
        }

        public (double, double[]) IterationsMethod()
        {
            double[] y;
            double[] yTemp = new double[rank];
            for (int i = 0; i < rank; i++)
            {
                yTemp[i] = 1;
            }

            double h = 0;
            double hTemp;
            do
            {
                hTemp = h;
                y = this * yTemp;
                h = y[0] / yTemp[0];
                yTemp = y;
            } while (Math.Abs(h - hTemp) >= 0.000001);
            for (int i = 0; i < rank; i++)
            {
                y[i] /= y[rank - 1];
            }
            return (h, y);
        }

    }
    static class DoubleExtension
    {
        public static double[][] CopyThis(this double[][] arr)
        {
            double[][] tempMatrix = new double[arr.Length][];
            for (int i = 0; i < arr.Length; i++)
            {
                tempMatrix[i] = new double[arr.Length];
                for (int j = 0; j < arr.Length; j++)
                    tempMatrix[i][j] = arr[i][j];
            }
            return tempMatrix;
        }
    }
}
