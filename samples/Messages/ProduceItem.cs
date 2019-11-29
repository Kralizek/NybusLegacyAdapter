using System;
using Nybus;

namespace Messages
{
    public class ProduceItem : ICommand
    {
        public Guid ItemId { get; set; }

        public float Quantity { get; set; }
    }
}
