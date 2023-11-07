namespace DatabaseExtended.Tests
{
    using ExtendedDatabase;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class ExtendedDatabaseTests
    {
        private Database database;

        [SetUp]
        public void Setup()
        {
            Person[] people =
            {
                new Person(1, "Savev"),
                new Person(2, "Valchev"),
                new Person(3, "Stoyan"),
                new Person(4, "StoSho"),
                new Person(5, "GoShow"),
                new Person(6, "Viktor"),
                new Person(7, "Lakova"),
                new Person(8, "Gergana"),
                new Person(9, "Georgi"),
                new Person(10, "Andonov"),
                new Person(11, "Sandov"),
                new Person(12, "Irena"),
                new Person(13, "Milena"),
        };

            database = new Database(people);
        }

        [Test] //1
        public void ConstructorCheckIfAnArrayIsCorrect()
        {
            int expectedCount = 13;
            Assert.AreEqual(expectedCount, database.Count);
        }

        [Test] //2
        public void ConstructorCheckIfElementsAreMoreThan16ThrowsAnException()
        {
            Person[] people = new Person[]
            {
                new(1, "Savev"),
                new(2, "Valchev"),
                new(3, "Stoyan"),
                new(4, "StoSho"),
                new(5, "GoShow"),
                new(6, "Viktor"),
                new(7, "Lakova"),
                new(8, "Gergana"),
                new(9, "Georgi"),
                new(10, "Andonov"),
                new(11, "Sandov"),
                new(12, "Irena"),
                new(13, "Milena"),
                new(14, "Petko"),
                new(15, "Petya"),
                new(16, "Stefan"),
                new(17, "Timotei")
            };

            ArgumentException exception = Assert.Throws<ArgumentException>(()
                 => database = new Database(people), "Provided data length should be in range [0..16]!");
        }

        [Test] //3
        public void DatabaseCountShouldWorkCorrectly()
        {
            Person[] people = new Person[]
            {
                new Person(1, "Savev"),
                new Person(2, "Valchev"),
                new Person(3, "Stoyan"),
                new Person(4, "StoSho"),
                new Person(5, "GoShow"),
                new Person(6, "Viktor"),
                new Person(7, "Lakova"),
                new Person(8, "Gergana"),
                new Person(9, "Georgi"),
                new Person(10, "Andonov"),
                new Person(11, "Sandov"),
                new Person(12, "Irena"),
                new Person(13, "Milena"),
            };

            Database database = new(people);

            int expectedResult = 13;
            int actualResult = database.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test] //4
        public void DatabaseAddMethodShouldIncreaseCount()
        {
            Person person = new Person(25, "Engivar");
            database.Add(person);

            int expectedResult = 14;
            int actualResult = database.Count;

            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test] //5
        public void AddPersonIfNotExists()
        {
            Person person = new(14, "Plamen");
            database.Add(person);

            int expectedCount = 14;
            Assert.AreEqual(expectedCount, database.Count);
        }

        [Test] //6
        public void AddPersonIfPeopleAreMoreThan16ThrowsAnException()
        {
            Person personOne = new(14, "Plamen");
            Person personTwo = new(15, "Petko");
            Person personThree = new(16, "Petya");
            Person personFour = new(17, "Timotei");

            database.Add(personOne);
            database.Add(personTwo);
            database.Add(personThree);

            Assert.Throws<InvalidOperationException>
              (() => database.Add(personFour), "Array's capacity must be exactly 16 integers!");
        }

        [Test] //7
        public void AddPersonThatExistsThrowsAnExceptionWithTheUsername()
        {
            Person person = new(20, "Savev");

            Assert.Throws<InvalidOperationException>
                 (() => database.Add(person), "There is already user with this username!");
        }

        [Test] //8
        public void AddPersonThatExistsThrowsAnExceptionWithTheId()
        {
            Person person = new(10, "Ivo");

            Assert.Throws<InvalidOperationException>
                 (() => database.Add(person), "There is already user with this Id!");
        }

        [Test] //9
        public void RemovePersonIfExists()
        {
            database.Remove();

            int expectedCount = 12;
            Assert.AreEqual(expectedCount, database.Count);
        }

        [Test] //10
        public void RemovePersonIfThereAreNoPeopleThrowsAnException()
        {
            Person[] people = new Person[]
           {

           };

            Database database = new(people);

            Assert.Throws<InvalidOperationException>
                 (() => database.Remove());
        }

        [Test] //11
        public void FindByUsernameIfExists()
        {
            string expectedPerson = "Savev";
            string currentPerson = database.FindByUsername("Savev").UserName;

            Assert.AreEqual(expectedPerson, currentPerson);
        }

        [TestCase("")] //12
        [TestCase(null)]
        public void FindByUsernameIfNullThrowsAnException(string username)
        {
            Assert.Throws<ArgumentNullException>
                 (() => database.FindByUsername(username), "Username parameter is null!");
        }

        [Test] //13
        public void FindByUsernameIfItIsCaseSensitive()
        {
            string expectedPerson = "SaveV";
            string currentPerson = database.FindByUsername("Savev").UserName;

            Assert.AreNotEqual(expectedPerson, currentPerson);
        }

        [TestCase("Engivar")] //14
        [TestCase("Angel")]
        public void FindByUsernameShouldThrowExceptionIfUsernameIsNotFound(string username)
        {
            InvalidOperationException exception = Assert.Throws<InvalidOperationException>(()
                => database.FindByUsername(username), "No user is present by this username!");
        }

        [TestCase(10)] //15
        public void FindByIdIfExists(long id)
        {
            long expectedId = id;
            long currentId = database.FindById(id).Id;
            //string expectedResult = "Savev";
            //string actualResult = database.FindById(10).UserName;

            //Assert.AreEqual(expectedResult, actualResult);

            Assert.AreEqual(expectedId, currentId);
        }

        [TestCase(-10)] //16
        public void FindByIdIfNegativeThrowsAnException(long id)
        {
            Assert.Throws<ArgumentOutOfRangeException>
                 (() => database.FindById(id), "Id should be a positive number!");
        }

        [TestCase(19)] //17
        public void FindByIdIfItNotExistsThrowsAnException(long id)
        {
            Assert.Throws<InvalidOperationException>
                 (() => database.FindById(id), "No user is present by this ID!");
        }
    }
}