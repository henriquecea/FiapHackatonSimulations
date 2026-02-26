using FiapHackatonSimulations.Application.Service;
using FiapHackatonSimulations.Domain.Entity;
using FiapHackatonSimulations.Domain.Interface.Repository;
using FiapHackatonSimulations.Tests.Mocks.DTO;
using FiapHackatonSimulations.Tests.Mocks.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace FiapHackatonSimulations.Tests.Service;

[TestFixture]
public class SimulationServiceTests
{
    #region Private Fields

    private Mock<ISimulationRepository> _repositoryMock;
    private Mock<ILogger<SimulationService>> _loggerMock;

    private SimulationService _service;

    #endregion

    #region Setup

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<ISimulationRepository>();
        _loggerMock = new Mock<ILogger<SimulationService>>();

        _service = new SimulationService(
            _repositoryMock.Object,
            _loggerMock.Object
        );
    }

    #endregion

    #region GetPlotsById

    [Test, Category("Success")]
    public async Task GetPlotsById_ShouldReturnOk_WhenPlotExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        var sensorData = new List<SensorData>
        {
            SendorDataMock.CreateValid()
        };

        _repositoryMock
            .Setup(r => r.GetByPlotIdAsync(id))
            .ReturnsAsync(sensorData);

        // Act
        var result = await _service.GetPlotsById(id);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());

        var okResult = result as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(okResult!.StatusCode, Is.Null.Or.EqualTo(200));
            Assert.That(okResult.Value, Is.Not.Null);
        });
    }

    [Test, Category("Success")]
    public async Task GetPlotsById_ShouldReturnNotFound_WhenPlotDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByPlotIdAsync(id))
            .ReturnsAsync((IEnumerable<SensorData>)null!);

        // Act
        var result = await _service.GetPlotsById(id);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundResult>());
    }

    [Test, Category("Error")]
    public async Task GetPlotsById_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByPlotIdAsync(id))
            .ThrowsAsync(new Exception("erro"));

        // Act
        var result = await _service.GetPlotsById(id);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        var badRequest = result as BadRequestObjectResult;

        Assert.That(badRequest!.Value, Is.EqualTo("erro"));
    }

    #endregion

    #region PostSimulationsData

    [Test, Category("Success")]
    public async Task PostSimulationsData_ShouldReturnAccepted_WhenRequestIsValid()
    {
        // Arrange
        var dto = SimulationDtoMock.CreateValid();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SensorData>()))
            .Returns(Task.CompletedTask);

        _repositoryMock
            .Setup(r => r.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.PostSimulationsData(dto);

        // Assert
        Assert.That(result, Is.TypeOf<AcceptedResult>());

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<SensorData>()), Times.Exactly(10));
        _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Test, Category("Error")]
    public async Task PostSimulationsData_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var dto = SimulationDtoMock.CreateValid();

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SensorData>()))
            .ThrowsAsync(new Exception("erro ao salvar"));

        // Act
        var result = await _service.PostSimulationsData(dto);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());

        var badRequest = result as BadRequestObjectResult;

        Assert.That(badRequest!.Value, Is.EqualTo("erro ao salvar"));
    }

    #endregion
}
