using System;

namespace Confluxx.StringFlection
{
	public static class ObjectStringFlector
	{
		public static object GetValue(object source, string path)
		{
			return GetValue(source, path, 0);
		}

		private static object GetValue(object source, string path, int startIndex)
		{
			if (path[startIndex] == '[')
			{
				var indexedProperty = GetValueByIndex(source, path, startIndex);
				var closingBracketIndex = path.IndexOf(']', startIndex);

				if (closingBracketIndex < path.Length - 1)
				{
					return GetValue(indexedProperty, path, closingBracketIndex + 1);
				}
				return indexedProperty;
			}

			if (path[startIndex] == '.')
			{
				startIndex++;
			}

			var sourceType = source.GetType();

			var boundaryIndex = path.IndexOfAny(BoundaryStartChars, startIndex);

			var currentPropertyName = boundaryIndex > -1
										? path.Substring(startIndex, boundaryIndex - startIndex)
										: path.Substring(startIndex);

			var propertyInfo = sourceType.GetProperty(currentPropertyName);
			if (propertyInfo == null)
			{
				throw new Exception();
			}

			var currentProperty = propertyInfo.GetValue(source, null);

			if (currentPropertyName.Length + startIndex == path.Length)
			{
				return currentProperty;
			}

			return GetValue(currentProperty, path, boundaryIndex);
		}

		private static object GetValueByIndex(object source, string path, int startIndex)
		{
			var sourceType = source.GetType();

			var boundaryEndIndex = path.IndexOf(']', startIndex);

			var indexString = path.Substring(startIndex + 1, boundaryEndIndex - startIndex - 1);

			if (sourceType.IsArray)
			{
				return ((Array) source).GetValue(int.Parse(indexString));
			}

			if (sourceType == typeof(string))
			{
				return ((string) source)[int.Parse(indexString)];
			}

			var index = indexString.IndexOfAny(StringBoundaryChars) == 0 
			                 	? new object[] { indexString.Trim(StringBoundaryChars) } 
			                 	: new object[] {int.Parse(indexString)};

			var propertyInfo = sourceType.GetProperty("Item");
			if (propertyInfo == null)
			{
				throw new Exception();
			}

			return propertyInfo.GetValue(source, index);
		}

		private static readonly Char[] BoundaryStartChars = new[] { '.', '[' };
		
		private static readonly Char[] StringBoundaryChars = new [] {'\'', '"'};
	}
}