using System;

namespace Notrosoft_Accel.Infrastructure
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Initialize the factorial cache.
            var _ = Utilities.Factorial(0);

            // Find the max factorial value possible.
            for (var i = 170; i < int.MaxValue; i++) Console.WriteLine($"{i}! = {Utilities.Factorial(i)}");
        }
    }
}