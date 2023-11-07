using System.Text;

namespace ShoeStore;

public class ShoeStore
{
    public ShoeStore(string name, int storageCapacity)
    {
        Name = name;
        StorageCapacity = storageCapacity;
        Shoes = new();
    }

    public string Name { get; set; }
    public int StorageCapacity { get; set; }
    public List<Shoe> Shoes { get; set; }
    public int Count { get { return Shoes.Count; } }

    public string AddShoe(Shoe shoe)
    {
        if (Count == StorageCapacity)
        {
            return "No more space in the storage room.";
        }

        Shoes.Add(shoe);
        return $"Successfully added {shoe.Type} {shoe.Material} pair of shoes to the store.";
    }

    public int RemoveShoes(string material)
    {
        int count = 0;

        for (int i = 0; i < Shoes.Count; i++)
        {
            if (Shoes[i].Material == material)
            {
                count++;
                Shoes.Remove(Shoes[i]);
                i = -1;
            }
        }

        return count;
    }

    public List<Shoe> GetShoesByType(string type)
    {
        List<Shoe> shoesType = new();

        foreach (var item in Shoes)
        {
            if (item.Type == type)
            {
                shoesType.Add(item);
            }
        }

        return shoesType;
    }

    public Shoe GetShoeBySize(double size)
    {
        Shoe shoe = Shoes.Where(x => x.Size == size).FirstOrDefault();
        return shoe;
    }

    public string StockList(double size, string type)
    {
        int count = 0;
        StringBuilder sb = new();
        sb.AppendLine($"Stock list for size {size} - {type} shoes:");

        foreach (var item in Shoes)
        {
            if (item.Size == size && item.Type == type)
            {
                count++;
                sb.AppendLine(item.ToString());
            }
        }

        if (count == 0)
        {
            return "No matches found!";
        }

        return sb.ToString().TrimEnd();
    }
}