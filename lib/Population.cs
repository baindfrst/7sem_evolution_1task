namespace lib;

public class Population
{
    public int numEpoch { get; set; }
    public Genom[] genArray { get; set; }
    public Genom bestGen { get; set; }
    public List<int> genomsresult { get; set; }
    public int[][] WayLengMap { get; set; }
    public int resultsolution = -1;
    public int learningRate = 0;
    public int numPopulation;

    /// <summary>
    /// Эта функция создает популяию
    /// </summary>
    /// <param name="count_population">Количество генов в популяции</param>
    /// <param name="cityMap">Матрица расстояний между городами</param>
    /// <param name="lr">максимальный кол-во мутаций</param>
    public Population(int count_population, int[][] cityMap, int lr, int numPopulation = 1)
    {
        learningRate = lr;
        genomsresult = new List<int>();
        numEpoch = 1;
        genArray = new Genom[count_population];
        for (int i = 0; i < count_population; i++)
        {
            genArray[i] = new Genom(cityMap.Length);
        }


        WayLengMap = new int[cityMap.Length][];
        for (int i = 0; i != cityMap.Length; i++)
        {
            WayLengMap[i] = new int[cityMap.Length];
            WayLengMap[i] = (int[])cityMap[i].Clone();
        }
        bestGen = genArray[this.CalculateSolutionsLenght()];
        this.numPopulation = numPopulation;
    }

    public int CalculateSolutionsLenght()
    {
        genomsresult.Clear();
        foreach (Genom gen in genArray)
        {
            genomsresult.Add(gen.CalculateGenomWayLenght(WayLengMap));
        }

        return genomsresult.IndexOf(genomsresult.Min());
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
            this.MutateRandomGen();
        }
        this.CalculateSolutionsLenght();
        numEpoch += 1;
        return numEpoch;

    }

    public string GetEpochResult()
    {
        return $"population number {numPopulation}: Epoch: {numEpoch}, best result is: {this.resultsolution} at genom\n {this.bestGen}\n";
    }

    public void StartPopulationEvolution(ref Genom outputGenom)
    {
        this.CalculateSolutionsLenght();
        resultsolution = this.genomsresult.Min();
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
            if(resultsolution > this.genomsresult.Min())
            {
                resultsolution = this.genomsresult.Min();
            }
            if (outputGenom.CalculateGenomWayLenght(WayLengMap) > this.genomsresult.Min())
            {
                resultsolution = this.genomsresult.Min();
                outputGenom = this.genArray[genomsresult.IndexOf(resultsolution)].ClonePopulation();
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
        int i_genom_new = genomsresult.IndexOf(genomsresult.Max());
        genArray[i_genom_new] = new Genom(WayLengMap.Length);
    }

    private void MutateRandomGen()
    {
        genArray[new Random().Next(0, genArray.Length)].GenomMutation();
    }

}
