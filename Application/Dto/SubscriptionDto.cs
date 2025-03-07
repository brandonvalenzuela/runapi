using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Dto
{
    public class SubscriptionDto
    {
        
    }
    
    // DTOs para requests
    public class CreateSubscriptionRequest
    {
        public SubscriptionType Type { get; set; }
        public decimal Price { get; set; }
        public SubscriptionPeriod Period { get; set; }
    }

    public class RenewSubscriptionRequest
    {
        public SubscriptionType NewType { get; set; }
        public decimal NewPrice { get; set; }
        public SubscriptionPeriod Period { get; set; }
    }
}
