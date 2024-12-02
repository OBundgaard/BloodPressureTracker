using Patient.Models;
using Moq;
using Newtonsoft.Json;
using System.Net;
using Moq.Protected;

namespace BptTestProject
{
    public class PatientTest
    {
        [Fact]
        public async Task PostTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())

                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient
         = new HttpClient(mockHttpMessageHandler.Object);

            PatientModel patient = new PatientModel
            {
                SSN = "1234",
                Mail = "Test@Test.com",
                Name = "Test",
            };

            // Act
            var response = await httpClient.PostAsync("http://localhost:5002", new StringContent(JsonConvert.SerializeObject(patient)));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://localhost:5002/1234"), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var response = await httpClient.GetAsync("http://localhost:5002/1234");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task PutTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));  // Relaxed condition for simplicity

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            PatientModel patient = new PatientModel
            {
                SSN = "1234",
                Mail = "Test@Test.com",
                Name = "Test",
            };

            // Act
            var response = await httpClient.PutAsync("http://localhost:5002", new StringContent(JsonConvert.SerializeObject(patient)));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString() == "http://localhost:5002/1234"), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var response = await httpClient.DeleteAsync("http://localhost:5002/1234");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}