//4 Exercise - Fast Food
int foodQuantity = int.Parse(Console.ReadLine());
Queue<int> orders = new(Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse));

Console.WriteLine(orders.Max());

while (orders.Any())
{
    if (foodQuantity < orders.Peek())
    {
        Console.WriteLine($"Orders left: {string.Join(" ", orders)}");
        break;
    }

    int currentOrder = orders.Dequeue();
    foodQuantity -= currentOrder;
}

if (orders.Count == 0)
{
    Console.WriteLine("Orders complete");
}
