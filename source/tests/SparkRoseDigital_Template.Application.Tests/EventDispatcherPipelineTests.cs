using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SparkRoseDigital.Infrastructure.MessageBroker;
using SparkRoseDigital_Template.Application.Pipelines;
using SparkRoseDigital_Template.Application.Tests.Helpers;
using Xunit;

namespace SparkRoseDigital_Template.Application.Tests
{
    public class EventDispatcherPipelineTests
    {
        [Fact]
        public async Task EventDispatcherPipeline_CallsNextThenPublishesAll_Successfully()
        {
            // Arrange
            var requestHandlerDelegateMock = new Mock<RequestHandlerDelegate<Response>>();
            var eventPublisherMock = new Mock<IRegisteredEventPublisher>();
            eventPublisherMock.Setup(m => m.PublishAll()).Returns(Task.CompletedTask);
            var target = new EventDispatcherPipeline<Request, Response>(eventPublisherMock.Object);

            // Act
            var result = await target.Handle(new Request(), default(CancellationToken), requestHandlerDelegateMock.Object);

            // Assert
            requestHandlerDelegateMock.Verify(m => m(), Times.Once);
            eventPublisherMock.Verify(m => m.PublishAll(), Times.Once);
        }

        [Fact]
        public async Task EventDispatcherPipeline_ReturnsResponse_Successfully()
        {
            // Arrange
            var eventPublisherMock = new Mock<IRegisteredEventPublisher>();
            eventPublisherMock.Setup(m => m.PublishAll()).Returns(Task.CompletedTask);
            var response = new Response("Test Response");
            //static Task<Response> requestHandlerDelegate() => Task.FromResult(response);
            RequestHandlerDelegate<Response> requestHandlerDelegate = () => Task.FromResult(response);
            var target = new EventDispatcherPipeline<Request, Response>(eventPublisherMock.Object);

            // Act
            var result = await target.Handle(new Request(), default(CancellationToken), requestHandlerDelegate);

            // Assert
            Assert.Equal(response, result);
        }

        [Fact]
        public async Task EventDispatcherPipeline_OnException_DoesNothing()
        {
            // Arrange
            var eventPublisherMock = new Mock<IRegisteredEventPublisher>();
            eventPublisherMock.Setup(m => m.PublishAll()).Returns(Task.CompletedTask);
            RequestHandlerDelegate<Response> requestHandlerDelegate = () => throw new Exception("");
            var target = new EventDispatcherPipeline<Request, Response>(eventPublisherMock.Object);

            // Act, Assert
            await Assert.ThrowsAsync<Exception>(
                () => target.Handle(new Request(), default(CancellationToken), requestHandlerDelegate));
        }
    }
}
