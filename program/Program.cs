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
        int sizeMatrix = 20;

        int[][] testmap = GenMatrixCity(sizeMatrix);

        Population testpopulation = new Population(30, testmap, 10);
        testpopulation.StartPopulationEvolution();
    }
    static public int[][] GenMatrixCity(int size)
    {
        int[][] map = new int[size][];

        for( int i = 0; i < size;i++)
        {
            map[i] = new int[size];
            Random random = new Random();
            for( int j = 0; j < size; j++)
            {
                map[i][j] = random.Next(0, 100);
            }
        }
        return map;
    }
}
