using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluxx.StringFlection;

namespace Confluxx.StringFlection.Demo
{
    class Program
    {
        private static Person GetSampleObject()
        {
            var result = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 42,
                Pets = new[] { "Dog", "Cat", "Horse" }
            };

            result.Children.Add(
                new Person
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Age = 3
                });

            result.Children.Add(
                new Person
                {
                    FirstName = "Alice",
                    LastName = "Doe",
                    Age = 6
                });

            result.Children.Add(
                new Person
                {
                    FirstName = "Bob",
                    LastName = "Doe",
                    Age = 8
                });

            result.PhoneNumbers["Home"] = "555-007";
            result.PhoneNumbers["Office"] = "555-23";

            return result;
        }

        public static void Main(string[] args)
        {
            Person person = GetSampleObject();

            Console.WriteLine(person.ToPropertyString("{FirstName} {LastName} has {Children.Count} children."));

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
