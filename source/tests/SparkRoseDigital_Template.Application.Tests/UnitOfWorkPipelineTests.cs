using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SparkRoseDigital_Template.Application.Pipelines;
using SparkRoseDigital_Template.Common.Interfaces;
using Xunit;

namespace SparkRoseDigital_Template.Application.Tests;

public class UnitOfWorkPipelineTests
{
    [Fact]
    public async Task UnitOfWorkPipeline_CallsNextThenSaves_Successfully()
    {
        // Arrange
        var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Response>>();
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(m => m.SaveAsync()).Returns(Task.FromResult(1));
        var target = new UnitOfWorkPipeline<Request, Response>(uowMock.Object);

        // Act
        var result = await target.Handle(new Request(), requestHandlerDelegateMock.Object, default(CancellationToken));

        // Assert
        requestHandlerDelegateMock.Verify(m => m(), Times.Once);
        uowMock.Verify(m => m.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task UnitOfWorkPipeline_ReturnsResponse_Successfully()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(m => m.SaveAsync()).Returns(Task.FromResult(1));
        var response = new Response("Test Response");
        RequestHandlerDelegate<Response> requestHandlerDelegate = () => Task.FromResult(response);
        var target = new UnitOfWorkPipeline<Request, Response>(uowMock.Object);

        // Act
        var result = await target.Handle(new Request(), requestHandlerDelegate, default(CancellationToken));

        // Assert
        Assert.Equal(response, result);
    }

    [Fact]
    public async Task UnitOfWorkPipeline_OnException_DoesNothing()
    {
        // Arrange
        var uowMock = new Mock<IUnitOfWork>();
        uowMock.Setup(m => m.SaveAsync()).Returns(Task.FromResult(1));
        RequestHandlerDelegate<Response> requestHandlerDelegate = () => throw new Exception("");
        var target = new UnitOfWorkPipeline<Request, Response>(uowMock.Object);

        // Act, Assert
        await Assert.ThrowsAsync<Exception>(
            () => target.Handle(new Request(), requestHandlerDelegate, default(CancellationToken)));
    }

    public class Request
    {
    }

    public class Response
    {
        public string ResponseText { get; set; }

        public Response(string responseText)
        {
            ResponseText = responseText;
        }
    }
}
