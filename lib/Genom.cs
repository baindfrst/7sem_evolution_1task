namespace lib;

public class Genom
{
    public int[] cityNumberConections { get; set; }
    public int GenomScore { get; set; }

    public Genom(int GenomScore, int[] cityNumberConections)
    {
        this.GenomScore = GenomScore;
        this.cityNumberConections = new int[cityNumberConections.Length];
        cityNumberConections.CopyTo(this.cityNumberConections, 0);
    }
    public Genom(int len_genom)
    {
        cityNumberConections = new int[len_genom];
        for (int i = 0; i < len_genom; i++)
        {
            cityNumberConections[i] = -1;
        }
        cityNumberConections[len_genom - 1] = 0;
        Random random = new Random();
        for (int i = 0; i < len_genom - 1; i++)
        {
            int next_station = random.Next(1, len_genom);
            while (Array.IndexOf(cityNumberConections, next_station) != -1)
            {
                next_station = random.Next(1, len_genom);
            }
            cityNumberConections[i] = next_station;
        }
    }

    public int CalculateGenomWayLenght(int[][] CityMap)
    {
        int GenomScoreInFunc = CityMap[0][cityNumberConections[0]];
        for (int i = 1; i < cityNumberConections.Length; i++)
        {
            GenomScoreInFunc += CityMap[cityNumberConections[i-1]][cityNumberConections[i]];
        }
        GenomScore = GenomScoreInFunc;
        return GenomScoreInFunc;
    }

    public void GenomMutation()
    {
        Random random = new Random();
        int frstPositiom = random.Next(0, cityNumberConections.Length - 1);
        int scndPosition = random.Next(0, cityNumberConections.Length - 1);
        while (scndPosition == frstPositiom)
        {
            scndPosition = random.Next(0, cityNumberConections.Length - 1);
        }
        int tmp = cityNumberConections[frstPositiom];
        cityNumberConections[frstPositiom] = cityNumberConections[scndPosition];
        cityNumberConections[scndPosition] = tmp;
    }

    public override string ToString()
    {
        return "0->" + string.Join("->", cityNumberConections);
    }
    public Genom ClonePopulation()
    {
        return new Genom(GenomScore, cityNumberConections);
    }
}
