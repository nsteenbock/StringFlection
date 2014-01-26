using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Confluxx.StringFlection.SampleClasses;

namespace Confluxx.StringFlection
{
    [TestClass]
	public class ToPropertyStringTest
	{
		private Person sampleObject;

		[TestInitialize]
		public void Init()
		{
			sampleObject = GetSampleObject();
		}

        [TestMethod]
        public void One_Property()
        {
            Assert.AreEqual("Hello my name is John.", sampleObject.ToPropertyString("Hello my name is {FirstName}."));
        }

        [TestMethod]
        public void Two_Properties()
        {
            Assert.AreEqual("Hello my name is John Doe.", sampleObject.ToPropertyString("Hello my name is {FirstName} {LastName}."));
        }

        [TestMethod]
        public void Property_Chain_With_2_Levels()
        {
            Assert.AreEqual("I have 3 children.", sampleObject.ToPropertyString("I have {Children.Count} children."));
        }
                
        [TestMethod]
        public void Property_Chain_With_Array()
        {
            Assert.AreEqual("My children's names are Jane, Alice and Bob.", sampleObject.ToPropertyString("My children's names are {Children[0].FirstName}, {Children[1].FirstName} and {Children[2].FirstName}."));
        }

        [TestMethod]
        public void Property_Chain_With_Dictionary()
        {
            Assert.AreEqual(
                "My home phone number is 555-007 and my office phone number is 555-23.",
                sampleObject.ToPropertyString("My home phone number is {PhoneNumbers[\"Home\"]} and my office phone number is {PhoneNumbers[\"Office\"]}."));
        }

        [TestMethod]
        public void Format_String_For_Int()
        {
            Assert.AreEqual("I have 003 children.", sampleObject.ToPropertyString("I have {Children.Count:000} children."));
        }

        [TestMethod]
        public void Format_String_For_DateTime()
        {
            Assert.AreEqual("My special day is 2014-01-26.", sampleObject.ToPropertyString("My special day is {SpecialDay:yyyy-MM-dd}."));
        }
        
        private Person GetSampleObject()
        {
            var result = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 42,
                Pets = new[] { "Dog", "Cat", "Horse" },
                SpecialDay = new DateTime(2014, 1, 26)
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
    }
}
