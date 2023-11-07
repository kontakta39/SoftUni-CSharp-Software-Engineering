using System.Text;

namespace ClothesMagazine;

public class Magazine
{
    public Magazine(string type, int capacity)
    {
        Type = type;
        Capacity = capacity;
        Clothes = new();
    }

    public void AddCloth(Cloth cloth)
    {
        if (Clothes.Count < Capacity)
        {
            Clothes.Add(cloth);
        }
    }

    public bool RemoveCloth(string color)
    {
        Cloth cloth = Clothes.Where(x => x.Color == color).FirstOrDefault();
        bool isContained = Clothes.Remove(cloth);
        return isContained;
    }

    public Cloth GetSmallestCloth()
    {
        Cloth cloth = Clothes.OrderBy(x => x.Size).FirstOrDefault();
        return cloth;
    }

    public Cloth GetCloth(string color)
    {
        Cloth cloth = Clothes.Where(x => x.Color == color).FirstOrDefault();
        return cloth;
    }

    public int GetClothCount()
    {
        return Clothes.Count;
    }

    public string Report()
    {
        StringBuilder sb = new();

        sb.AppendLine($"{Type} magazine contains:");

        foreach (Cloth cloth in Clothes.OrderBy(x => x.Size))
        {
            sb.AppendLine(cloth.ToString());
        }

        return sb.ToString().TrimEnd();
    }

    public string Type { get; set; }
    public int Capacity { get; set; }
    public List<Cloth> Clothes { get; set; }
}