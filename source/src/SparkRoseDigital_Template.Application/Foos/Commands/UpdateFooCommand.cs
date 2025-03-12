using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SparkRoseDigital_Template.Application.Foos.Commands
{
    public class UpdateFooCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        private class UpdateFooCommandHandler : IRequestHandler<UpdateFooCommand, Unit>
        {
            public UpdateFooCommandHandler()
            {
            }

            public Task<Unit> Handle(UpdateFooCommand request, CancellationToken cancellationToken) => Task.FromResult(Unit.Value);
        }
    }
}
