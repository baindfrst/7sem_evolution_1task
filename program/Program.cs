using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using lib;
using System.Runtime.InteropServices;

//ОСТАЛОСЬ РЕШИТЬ ПРОБЛЕМУ ТОГО, ЧТО ГЕНОМЫ КОТОРЫЕ СОЗДАЮТСЯ НЕ ОБЯЗАТЕЛЬНО ЯВЛЯЮТСЯ ПОЛНЫМ ЦИКЛОМ
internal class main
{
    static void Main(string[] args)
    {
        int sizeMatrix = 15;

        int[][] testmap = GenMatrixCity(sizeMatrix);

        for (int i = 0; i < sizeMatrix; i++)
        {
            for (int j = 0; j < sizeMatrix; j++)
            {
                Console.Write(testmap[i][j] + " ");
            }
            Console.WriteLine();
        }
        MultiPopulationWork MP = new MultiPopulationWork(2, 8, testmap, 4);
        MP.Run();
    }
    public static int[][] GenMatrixCity(int size)
    {
        Random random = new Random();
        int[][] matrix = new int[size][];
        for (int i = 0; i < size; i++)
        {
            matrix[i] = new int[size];
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                int distance = random.Next(1, 100);
                matrix[i][j] = distance;
                matrix[j][i] = distance;
            }
        }
        for (int i = 0; i < size; i++)
        {
            for (int j = i + 1; j < size; j++)
            {
                for (int k = j + 1; k < size; k++)
                {
                    if (matrix[i][j] + matrix[j][k] < matrix[i][k])
                    {
                        matrix[i][k] = matrix[i][j] + matrix[j][k] + 1;
                        matrix[k][i] = matrix[i][k];
                    }
                }
            }
        }

        return matrix;
    }
}
