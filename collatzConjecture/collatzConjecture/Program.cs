namespace collatzConjecture
{
    internal class Program
    {
        List<long> ReachedNumbers = new(1000000);
        Dictionary<string, long> Collatz(long num)
        {
            long count = 0;
            long max = num;
            while (num > 1)
            {
                if (ReachedNumbers.Contains(num))
                {
                    return new Dictionary<string, long> { { "length", -1 }, { "max", max } };
                }
                ReachedNumbers.Add(num);
                if ((num & 1) == 1)
                {
                    num = num * 3 + 1;
                }
                else
                {
                    num >>= 1;
                }
                ++count;
                if (num > max)
                {
                    max = num;
                }
            }
            return new Dictionary<string, long> { { "length", count }, { "max", max } };
        }
        static void Main(string[] args)
        {
            Console.Write("Enter the position to start the Collatz Conjecture at: ");
            var input = Console.ReadLine();
            Console.Write("\nEnter the position to end the Collatz Conjecture at:   ");
            var end = Console.ReadLine();
            if (input != null && end != null)
            {
                string i = input;
                string e = end;
                try
                {
                    long num = Int64.Parse(i);
                    long inum = num;
                    long num2 = Int64.Parse(e);

                    if (num <= 0 || num2 <= 0)
                    {
                        Console.WriteLine($"Please specify a positive number!");
                        return;
                    }
                    if (num > num2)
                    {
                        Console.WriteLine("Please have the second number larger than the first.");
                    }
                    Program p = new();
                    long longestCount = 0;
                    long lcn = 0;
                    long largestNum = 0;
                    long lnn = 0;
                    for (; num < num2; ++num)
                    {
                        Dictionary<string, long> res = p.Collatz(num);
                        if (res["length"] > longestCount)
                        {
                            lcn = num;
                            longestCount = res["length"];
                        }
                        if (res["max"] > largestNum)
                        {
                            lnn = num;
                            largestNum = res["max"];
                        }
                        if (num % 1000000 == 0)
                        {
                            Console.WriteLine($"{num - inum} iteration complete, {num2 - num} more to go. {((double)(num - inum + 1) / (num2 - inum) * 100).ToString("00.000000")}% complete.");
                        }
                    }
                    Console.WriteLine($"==========COMPLETE==========\n" +
                        $"Longest count was {longestCount} long, at n={lcn}.\n" +
                        $"Largest number was {largestNum}, at n={lnn}.");
                }
                catch (FormatException)
                {
                    Console.WriteLine($"Could not parse '{i}'.");
                }
            }
        }
    }
}

