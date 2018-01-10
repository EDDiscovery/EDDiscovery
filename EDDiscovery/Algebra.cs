using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scatter
{
    static class Algerbra
    {
        public class Matrix<T>
        {
            int rows;
            int columns;

            private T[,] matrix;

            public Matrix(int n, int m)
            {
                matrix = new T[n, m];
                rows = n;
                columns = m;
            }

            public void SetValByIdx(int m, int n, T x)
            {
                matrix[n, m] = x;
            }

            public T GetValByIndex(int n, int m)
            {
                return matrix[n, m];
            }

            public void SetMatrix(T[] arr)
            {
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < columns; c++)
                        matrix[r, c] = arr[r * columns + c];
            }

            public static Matrix<T> operator |(Matrix<T> m1, Matrix<T> m2)
            {
                Matrix<T> m = new Matrix<T>(m1.rows, m1.columns + m2.columns);
                for (int r = 0; r < m1.rows; r++)
                {
                    for (int c = 0; c < m1.columns; c++)
                        m.matrix[r, c] = m1.matrix[r, c];
                    for (int c = 0; c < m2.columns; c++)
                        m.matrix[r, c + m1.columns] = m2.matrix[r, c];
                }
                return m;
            }

            public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
            {
                Matrix<T> m = new Matrix<T>(m1.rows, m2.columns);
                for (int r = 0; r < m.rows; r++)
                    for (int c = 0; c < m.columns; c++)
                    {
                        T tmp = (dynamic)0;
                        for (int i = 0; i < m2.rows; i++)
                            tmp += (dynamic)m1.matrix[r, i] * (dynamic)m2.matrix[i, c];
                        m.matrix[r, c] = tmp;
                    }
                return m;
            }

            public static Matrix<T> operator ~(Matrix<T> m)
            {
                Matrix<T> tmp = new Matrix<T>(m.columns, m.rows);
                for (int r = 0; r < m.rows; r++)
                    for (int c = 0; c < m.columns; c++)
                        tmp.matrix[c, r] = m.matrix[r, c];
                return tmp;
            }

            public static Matrix<T> operator -(Matrix<T> m)
            {
                Matrix<T> tmp = new Matrix<T>(m.columns, m.rows);
                for (int r = 0; r < m.rows; r++)
                    for (int c = 0; c < m.columns; c++)
                        tmp.matrix[r, c] = -(dynamic)m.matrix[r, c];
                return tmp;
            }

            public override string ToString()
            {
                String output = "";
                for (int r = 0; r < rows; r++)
                {
                    output += "[\t";
                    for (int c = 0; c < columns; c++)
                    {
                        output += matrix[r, c].ToString();
                        if (c < columns - 1) output += ",\t";
                    }
                    output += "]\n";
                }
                return output;
            }
        }


    }
}
