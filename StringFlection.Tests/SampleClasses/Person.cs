using System.Collections.Generic;

namespace Confluxx.StringFlection.SampleClasses
{
	public class Person
	{
		public string FirstName { get; set; }

		public string LastName { get; set; }

		public int Age { get; set; }

		public List<Person> Children { get; protected set; }

		public Dictionary<string, string> PhoneNumbers { get; protected set; }

		public string[] Pets { get; set; }

		public Person()
		{
			this.Children = new List<Person>();
			this.PhoneNumbers = new Dictionary<string, string>();
		}
	}
}