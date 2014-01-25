using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Confluxx.StringFlection.SampleClasses;

namespace Confluxx.StringFlection
{
	[TestClass]
	public class ObjectStringFlectorTest
	{
		private Person sampleObject;

		[TestInitialize]
		public void Init()
		{
			sampleObject = GetSampleObject();
		}

		[TestMethod]
		public void GetValue_StringProperty()
		{
			Assert.AreEqual("John", ObjectStringFlector.GetValue(sampleObject, "FirstName"));
			Assert.AreEqual("Doe", ObjectStringFlector.GetValue(sampleObject, "LastName"));
		}

		[TestMethod]
		public void GetValue_IntProperty()
		{
			Assert.AreEqual(42, ObjectStringFlector.GetValue(sampleObject, "Age"));
		}

		[TestMethod]
		public void GetValue_ListProperty()
		{
			Assert.AreEqual(sampleObject.Children, ObjectStringFlector.GetValue(sampleObject, "Children"));
		}

		[TestMethod]
		public void GetValue_ListElementByIndex()
		{
			Assert.AreEqual(sampleObject.Children[1], ObjectStringFlector.GetValue(sampleObject, "Children[1]"));
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetValue_InvalidPropertyName()
		{
			ObjectStringFlector.GetValue(sampleObject, "NotExistingPropertyName");
		}

		[TestMethod]
		[ExpectedException(typeof(Exception))]
		public void GetValue_IndexOnNonIndexedProperty()
		{
			ObjectStringFlector.GetValue(sampleObject, "Age[2]");
		}

		[TestMethod]
		public void GetValue_PropertyDepthTwo()
		{
			Assert.AreEqual(3, ObjectStringFlector.GetValue(sampleObject, "Children.Count"));
		}

		[TestMethod]
		public void GetValue_PropertyOfIndexedObject()
		{
			Assert.AreEqual("Alice", ObjectStringFlector.GetValue(sampleObject, "Children[1].FirstName"));
		}

		[TestMethod]
		public void GetValue_DictionaryWithStringKeys()
		{
			Assert.AreEqual("555-23", ObjectStringFlector.GetValue(sampleObject, "PhoneNumbers[\"Office\"]"));
		}

		[TestMethod]
		public void GetValue_ArrayElementByIndex()
		{
			Assert.AreEqual("Cat", ObjectStringFlector.GetValue(sampleObject, "Pets[1]"));
		}

		[TestMethod]
		public void GetValue_CharFromStringByIndex()
		{
			Assert.AreEqual('a', ObjectStringFlector.GetValue(sampleObject, "Pets[1][1]"));
		}

		private Person GetSampleObject()
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
	}
}