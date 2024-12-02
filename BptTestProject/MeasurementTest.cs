using Measurement.Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace BptTestProject
{

    public class MeasurementTest
    {
        [Fact]
        public async Task APostTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())

                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient
         = new HttpClient(mockHttpMessageHandler.Object);

            MeasurementModel measurement = new MeasurementModel
            {
                Id = 1234,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Systolic = 1,
                Diastolic = 1,
                Seen = true
            };

            // Act
            var response = await httpClient.PostAsync("http://localhost:5000", new StringContent(JsonConvert.SerializeObject(measurement)));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task BGetTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString() == "http://localhost:5000/1234"), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var response = await httpClient.GetAsync("http://localhost:5000/1234");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CPutTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())

                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient
         = new HttpClient(mockHttpMessageHandler.Object);

            MeasurementModel measurement = new MeasurementModel
            {
                Id = 1234,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Systolic = 1,
                Diastolic = 1,
                Seen = true
            };

            // Act
            var response = await httpClient.PutAsync("http://localhost:5000", new StringContent(JsonConvert.SerializeObject(measurement)));

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


        [Fact]
        public async Task DeleteTestSuccess()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
         ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.ToString()
         == "http://localhost:5000/1234"), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            // Act
            var response = await httpClient.DeleteAsync("http://localhost:5000/1234");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }


    }
}