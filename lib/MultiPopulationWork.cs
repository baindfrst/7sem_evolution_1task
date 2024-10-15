namespace lib
{
    public class MultiPopulationWork
    {
        int countThreads;
        public List<Thread> threadsList;
        public Genom bestgen;

        public MultiPopulationWork(int countThread, int count_population, int[][] cityMap, int lr)
        {
            threadsList = new List<Thread>();
            bestgen = new Genom(cityMap.Length);
            bestgen.CalculateGenomWayLenght(cityMap);
            this.countThreads = countThread;
            for(int i = 0; i < countThreads; i++)
            {
                Population tm = new Population(count_population, cityMap, lr, i);
                threadsList.Add(new Thread(() => tm.StartPopulationEvolution(ref bestgen)));
                Console.WriteLine(threadsList.Count);
            }    
        }

        public void Run()
        {
            Console.WriteLine(bestgen.GenomScore);
            foreach(Thread threadt in threadsList)
            {
                threadt.Start();
            }
            foreach(Thread threadt in threadsList)
            {
                threadt.Join();
            }
            Console.WriteLine("FIN");
            Console.WriteLine(bestgen.GenomScore);
        }
    }
}