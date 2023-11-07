namespace UniversityLibrary.Test;

using NuGet.Frameworks;
using NUnit.Framework;
using System.Collections.Generic;

public class UniversityTests
{
    [Test]
    public void ConstructorCheck()
    {
        UniversityLibrary universityLibrary = new();
        Assert.IsNotNull(universityLibrary.Catalogue);
    }

    [Test]
    public void CheckListCatalogueSetter()
    {
        List<TextBook> library = new();
        UniversityLibrary universityLibrary = new();
        Assert.AreEqual(library.Count, universityLibrary.Catalogue.Count);
    }

    [Test]
    public void AddTextBookToLibraryCorrectly()
    {
        UniversityLibrary universityLibrary = new();
        TextBook textBook = new("The Parking", "Savev", "Thriller");
        textBook.InventoryNumber = 1;
        string expectedTextBook = textBook.ToString();

        TextBook textBookOne = new("The Parking", "Savev", "Thriller");
        string currentTextBook = universityLibrary.AddTextBookToLibrary(textBookOne);

        Assert.AreEqual(expectedTextBook, currentTextBook);
        Assert.AreEqual(1, universityLibrary.Catalogue.Count);
    }

    [Test]
    public void LoanTextBookButTheStudentHasNotReturnedTheBook()
    {
        UniversityLibrary universityLibrary = new();
        int currentInventoryNumber = universityLibrary.Catalogue.Count;
        string studentName = "Valchev";
        TextBook textBook = new("The Parking", "Savev", "Thriller");
        textBook.Holder = "Valchev";
        string expectedMessage = $"{studentName} still hasn't returned {textBook.Title}!";

        TextBook textBookOne = new("Galevi Brothers", "Galev", "Criminal");
        universityLibrary.AddTextBookToLibrary(textBook);
        universityLibrary.AddTextBookToLibrary(textBookOne);

        string currentMessage = universityLibrary.LoanTextBook(currentInventoryNumber + 1, studentName);

        Assert.AreEqual(currentInventoryNumber + 1, textBook.InventoryNumber);
        Assert.AreEqual(studentName, textBook.Holder);
        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(2, universityLibrary.Catalogue.Count);
    }

    [Test]
    public void LoanTextBookCorrectly()
    {
        UniversityLibrary universityLibrary = new();
        int currentInventoryNumber = universityLibrary.Catalogue.Count;
        string studentName = "Valchev";
        TextBook textBook = new("The Parking", "Savev", "Thriller");
        string expectedMessage = $"{textBook.Title} loaned to {studentName}.";

        TextBook textBookOne = new("Galevi Brothers", "Galev", "Criminal");
        universityLibrary.AddTextBookToLibrary(textBook);
        universityLibrary.AddTextBookToLibrary(textBookOne);

        string currentMessage = universityLibrary.LoanTextBook(currentInventoryNumber + 1, studentName);

        Assert.AreEqual(currentInventoryNumber + 1, textBook.InventoryNumber);
        Assert.AreEqual(studentName, textBook.Holder);
        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(2, universityLibrary.Catalogue.Count);
    }

    [Test]
    public void ReturnTextBookCorrectly()
    {
        UniversityLibrary universityLibrary = new();
        int currentInventoryNumber = universityLibrary.Catalogue.Count;
        TextBook textBook = new("The Parking", "Savev", "Thriller");
        textBook.Holder = "Valchev";
        string expectedMessage = $"{textBook.Title} is returned to the library.";

        TextBook textBookOne = new("Galevi Brothers", "Galev", "Criminal");
        universityLibrary.AddTextBookToLibrary(textBook);
        universityLibrary.AddTextBookToLibrary(textBookOne);

        string currentMessage = universityLibrary.ReturnTextBook(currentInventoryNumber + 1);

        Assert.AreEqual(currentInventoryNumber + 1, textBook.InventoryNumber);
        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(2, universityLibrary.Catalogue.Count);
    }

    [Test]
    public void ReturnTextBookCorrectlyOne()
    {
        UniversityLibrary universityLibrary = new();
        int currentInventoryNumber = universityLibrary.Catalogue.Count;
        TextBook textBook = new("The Parking", "Savev", "Thriller");
        string expectedMessage = $"{textBook.Title} is returned to the library.";

        TextBook textBookOne = new("Galevi Brothers", "Galev", "Criminal");
        universityLibrary.AddTextBookToLibrary(textBook);
        universityLibrary.AddTextBookToLibrary(textBookOne);

        string currentMessage = universityLibrary.ReturnTextBook(currentInventoryNumber + 1);

        Assert.AreEqual(currentInventoryNumber + 1, textBook.InventoryNumber);
        Assert.AreEqual("", textBook.Holder);
        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(2, universityLibrary.Catalogue.Count);
    }
}