namespace Database.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class DatabaseTests
    {
        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        public void ConstructorCheckIfAnArrayIsEqualTo16(int[] array)
        {
            Database database = new Database(array);

            int expectedCount = 16;
            Assert.AreEqual(expectedCount, database.Count);
        }

        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17})]
        public void ConstructorCheckIfElementsAreMoreThan16ThrowsAnException(int[] array)
        {
            Database database = null;

            Assert.Throws<InvalidOperationException>
                 (() => database = new Database(array), "Array's capacity must be exactly 16 integers!");
        }

        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 })]
        public void AddAnElementToAnArray(int[] array)
        {
            Database database = new Database(array);

            int addElement = 15;
            database.Add(addElement);

            int expectedArrayCount = 16;
            Assert.AreEqual(expectedArrayCount, database.Count);
        }

        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        public void AddAnElementToAnFullArrayThrowsAnException(int[] array)
        {
            Database database = new Database(array);

            int addElement = 16;

            Assert.Throws<InvalidOperationException>
                 (() => database.Add(addElement), "Array's capacity must be exactly 16 integers!");
        }

        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        public void RemoveAnElementFromAnArray(int[] array)
        {
            Database database = new Database(array);

            database.Remove();

            int expectedArrayCount = 15;
            Assert.AreEqual(expectedArrayCount, database.Count);
        }

        [TestCase(new int[] { 1 })]
        public void RemoveAnElementFromAnEmptyArrayThrowsAnException(int[] array)
        {
            Database database = new Database(array);

            database.Remove();

            Assert.Throws<InvalidOperationException>
                 (() => database.Remove(), "The collection is empty!");
        }

        [TestCase(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 })]
        public void FetchMethodWhichReturnsElementsInAnArray(int[] array)
        {
            Database database = new Database(array);

            int[] fetchedArray = database.Fetch();

            Assert.AreEqual(fetchedArray, array);
        }
    }
}
