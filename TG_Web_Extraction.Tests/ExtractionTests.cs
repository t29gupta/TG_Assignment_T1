using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;

namespace TG_Web_Extraction.Tests
{
    public class Tests
    {
        private string mockFilePath;
        private HTML_Extractiom extraction;

        [SetUp]
        public void Setup()
        {
            mockFilePath = Path.Combine(AppContext.BaseDirectory, "MockFile.html");
            extraction = new HTML_Extractiom();
        }

        [Test]
        public void ExtractionWithStringContent_NotNull()
        {
            /// Arrange
            // Read htmlFile file

            if (string.IsNullOrEmpty(mockFilePath))
            {
                Assert.Fail("Test Setup not run");
            }

            var htmlFileContent = File.ReadAllText(mockFilePath);

            ///Act
            var extractedData = extraction.HtmlExtractionFromStringContent(htmlFileContent);

            Assert.IsNotNull(extractedData);
        }

        [Test]
        public void ExtractionWithStream_NotNull()
        {
            /// Arrange
            // Read htmlFile file

            if (string.IsNullOrEmpty(mockFilePath))
            {
                Assert.Fail("Test Setup not run");
            }

            var extractedData = "";
            using (var htmlFileStream = File.OpenRead(mockFilePath))
            {
                ///Act
                extractedData = extraction.HtmlExtractionFromStream(htmlFileStream);
            }

            Assert.IsNotNull(extractedData);
        }

        [Test]
        public void ExtractionWithStringContent_JsonPropsPresent()
        {
            /// Arrange
            // Read htmlFile file

            if (string.IsNullOrEmpty(mockFilePath))
            {
                Assert.Fail("Test Setup not run");
            }

            var htmlFileContent = File.ReadAllText(mockFilePath);

            ///Act
            var extractedData = extraction.HtmlExtractionFromStringContent(htmlFileContent);

            var jsonDoc = JsonDocument.Parse(extractedData);

            JsonElement element;

            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("HotelName", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("Address", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("ReviewPoint", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("Classification_Stars", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("NoOfReviews", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("Description", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("RoomCategories", out element));
            Assert.IsTrue(jsonDoc.RootElement.TryGetProperty("AlternativeHotels", out element));
        }

        [Test]
        public void ExtractionWithStringContent_JsonPropsType()
        {
            /// Arrange
            // Read htmlFile file

            if (string.IsNullOrEmpty(mockFilePath))
            {
                Assert.Fail("Test Setup not run");
            }

            var htmlFileContent = File.ReadAllText(mockFilePath);

            ///Act
            var extractedData = extraction.HtmlExtractionFromStringContent(htmlFileContent);

            var jsonDoc = JsonDocument.Parse(extractedData);

            JsonElement element;
            if (jsonDoc.RootElement.TryGetProperty("HotelName", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("Address", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("ReviewPoint", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "object");
            }
            if (jsonDoc.RootElement.TryGetProperty("Classification_Stars", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }
            if (jsonDoc.RootElement.TryGetProperty("NoOfReviews", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("Description", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("RoomCategories", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "array");
            }

            if (jsonDoc.RootElement.TryGetProperty("AlternativeHotels", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "array");
            }
        }

        [Test]
        public void InvalidFileContent_JsonPropsAreNull()
        {
            string htmlFileContent = "";

            var extractedData = extraction.HtmlExtractionFromStringContent(htmlFileContent);

            var jsonDoc = JsonDocument.Parse(extractedData);

            JsonElement element;
            if (jsonDoc.RootElement.TryGetProperty("HotelName", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "null");
            }

            if (jsonDoc.RootElement.TryGetProperty("Address", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "null");
            }

            if (jsonDoc.RootElement.TryGetProperty("ReviewPoint", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "null");
            }
            if (jsonDoc.RootElement.TryGetProperty("Classification_Stars", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }
            if (jsonDoc.RootElement.TryGetProperty("NoOfReviews", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("Description", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "string");
            }

            if (jsonDoc.RootElement.TryGetProperty("RoomCategories", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "array");
            }

            if (jsonDoc.RootElement.TryGetProperty("AlternativeHotels", out element))
            {
                Assert.AreEqual(element.ValueKind.ToString().ToLower(), "array");
            }
        }

        [Test]
        public void NullFileContent_ReturnsEmpty()
        {
            var extractedData = extraction.HtmlExtractionFromStringContent(null);

            Assert.IsEmpty(extractedData);
        }

        [Test]
        public void NullFileStream_ReturnsEmpty()
        {
            var extractedData = extraction.HtmlExtractionFromStream(null);

            Assert.IsEmpty(extractedData);
        }
    }
}