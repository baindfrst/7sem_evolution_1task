namespace lib;

public class Population
{
    public int numEpoch { get; set; }
    public Genom[] genArray { get; set; }
    public List<int> genomsresult { get; set; }
    public int[][] WayLengMap { get; set; }
    public int resultsolution = -1;
    public int learningRate = 0;

    /// <summary>
    /// Эта функция создает популяию
    /// </summary>
    /// <param name="count_population">Количество генов в популяции</param>
    /// <param name="cityMap">Матрица расстояний между городами</param>
    /// <param name="lr">максимальный кол-во мутаций</param>
    public Population(int count_population, int[][] cityMap, int lr)
    {
        learningRate = lr;
        genomsresult = new List<int>();
        numEpoch = 1;
        genArray = new Genom[count_population];
        for (int i = 0; i < count_population; i++)
        {
            genArray[i] = new Genom(cityMap.Length);
        }
        WayLengMap = cityMap;
    }

    public int CalculateSolutionsScore()
    {
        genomsresult.Clear();
        foreach (Genom gen in genArray)
        {
            genomsresult.Add(gen.CalculateGenomScore(WayLengMap));
        }

        return genomsresult.Min();
    }


    public int GenNewEpoch()
    {
        Random random = new Random();
        int mutatitionLr = random.Next(0, learningRate);
        int deadGenomLr = random.Next(0, genArray.Length - mutatitionLr);

        for (int i = 0; i < deadGenomLr; i++)
        {
            this.CreateNewGenom();
        }
        for (int i = 0; i < mutatitionLr; i++)
        {
            this.CreateNewGenom();
        }

        numEpoch += 1;
        return numEpoch;

    }

    public string GetEpochResult()
    {
        return $"Epoch: {numEpoch}, best result is: {genomsresult.Min()} at genom\n {genArray[genomsresult.IndexOf(genomsresult.Min())]}\n";
    }

    public void StartPopulationEvolution()
    {
        resultsolution = this.CalculateSolutionsScore();
        bool loop = false;
        Console.CancelKeyPress += (sender, e) =>
        {
            e.Cancel = true;
            loop = true;
        };
        Console.WriteLine(this.GetEpochResult());
        while (!Console.KeyAvailable)
        {
            if (loop)
            {
                break;
            }
            this.GenNewEpoch();
            if (this.genomsresult.Min() < resultsolution)
            {
                resultsolution = this.genomsresult.Min();
                Console.WriteLine(this.GetEpochResult());
            }
        }
        Console.WriteLine("\n\n\nПодбор окончен");
        Console.WriteLine(this.GetEpochResult());
    }
    public override string ToString()
    {
        string outs = "";
        foreach (Genom gen in genArray)
        {
            outs += $"{gen.ToString()} \n\n";
        }
        return outs;
    }

    private void CreateNewGenom()
    {
        genArray[genomsresult.IndexOf(genomsresult.Max())] = new Genom(WayLengMap.Length);
        this.CalculateSolutionsScore();
    }

    private void MutateRandomGen()
    {
        genArray[new Random().Next(0, genArray.Length)].GenomMutation();
    }

}
