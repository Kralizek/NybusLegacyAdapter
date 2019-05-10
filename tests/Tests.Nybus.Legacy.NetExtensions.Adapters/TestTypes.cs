using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nybus;

namespace Tests
{
    public class TestClass { }

    public class TestCommand : ICommand { }

    public class TestEvent : IEvent { }

    public class TestCommandHandler : ICommandHandler<TestCommand>
    {
        public Task Handle(CommandContext<TestCommand> commandMessage)
        {
            throw new NotImplementedException();
        }
    }

    public class TestEventHandler : IEventHandler<TestEvent>
    {
        public Task Handle(EventContext<TestEvent> eventMessage)
        {
            throw new NotImplementedException();
        }
    }
}
