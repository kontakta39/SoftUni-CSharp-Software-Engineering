//11 Exercise - Key Revolver
int bulletPrice = int.Parse(Console.ReadLine());
int gunBarrelSize = int.Parse(Console.ReadLine());
Stack<int> bullets = new(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
Queue<int> locks = new(Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse));
int intelligenceValue = int.Parse(Console.ReadLine());
int expendedBulletsCount = 0;
int currentBarrelSize = gunBarrelSize;

while (bullets.Count > 0 && locks.Count > 0)
{
    int currentBulletSize = bullets.Pop();
    expendedBulletsCount++;
    int currentLockSize = locks.Peek();

    if (currentBulletSize <= currentLockSize)
    {
        Console.WriteLine("Bang!");
        currentBarrelSize--;
        locks.Dequeue();
    }

    else if (currentBulletSize > currentLockSize)
    {
        Console.WriteLine("Ping!");
        currentBarrelSize--;
    }

    if (currentBarrelSize == 0 && bullets.Count > 0)
    {
        currentBarrelSize = gunBarrelSize;
        Console.WriteLine("Reloading!");
    }
}

if (bullets.Count >= 0 && locks.Count == 0)
{
    int remainingBulletsCount = 0;

    foreach (var item in bullets)
    {
        remainingBulletsCount++;
    }

    int sum = expendedBulletsCount * bulletPrice;
    Console.WriteLine($"{remainingBulletsCount} bullets left. Earned ${intelligenceValue - sum}");
}

else
{
    int remainingLocksCount = 0;

    foreach (var item in locks)
    {
        remainingLocksCount++;
    }

    Console.WriteLine($"Couldn't get through. Locks left: {remainingLocksCount}");
}
