using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WorkTimer.Domain.Models;
using WorkTimer.MediatR.Handlers;
using WorkTimer.MediatR.Requests;
using WorkTimer.Persistence.Data;
using Xunit;

namespace WorkTimer.MediatRTests.Handlers
{
    public class IndexHandlerTests
    {
        private readonly IndexHandler _testObject;

        public IndexHandlerTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            using (var context = new AppDbContext(options))
            {
                context.WorkDays.AddRange(GetData());
                context.SaveChanges();
            }

            _testObject = new IndexHandler(new AppDbContext(options));
        }

        [Fact]
        public async Task Handler_Works()
        {
            var request = new IndexRequest() { User = new AppUser { Id = 1 } };

            var response = await _testObject.Handle(request, CancellationToken.None);
        }

        private WorkDay[] GetData()
        {
            var user = new AppUser { Id = 1 };

            var contract = new Contract { Id = 1, Employer = nameof(Contract.Employer), HoursPerWeek = 40, IsCurrent = true, Name = nameof(Contract.Name), UserId = 1, User = user };

            return new WorkDay[]
            {
                new() { Id = 1, ContractId = 1, Contract = contract, Date = new DateTime(2021, 02, 01), WorkDayType = WorkDayType.Workday, TotalHours = 7.5, WorkingPeriods = new List<WorkingPeriod>()
                {
                    new WorkingPeriod { Id = 1, StartTime = new DateTime(2021, 02, 01, 09, 0, 0), EndTime = new DateTime(2021, 02, 01, 12, 30, 0) },
                    new WorkingPeriod { Id = 2, StartTime = new DateTime(2021, 02, 01, 13, 0, 0), EndTime = new DateTime(2021, 02, 01, 17, 0, 0) }
                }  },
                new() { Id = 2, ContractId = 1, Contract = contract, Date = new DateTime(2021, 02, 02), WorkDayType = WorkDayType.Workday, TotalHours = 8.4167, WorkingPeriods = new List<WorkingPeriod>() {
                    new WorkingPeriod { Id = 3, StartTime = new DateTime(2021, 02, 02, 8, 20, 0), EndTime = new DateTime(2021, 02, 02, 12, 50, 0) },
                    new WorkingPeriod { Id = 4, StartTime = new DateTime(2021, 02, 02, 15, 40, 0), EndTime = new DateTime(2021, 02, 02, 17, 10, 0) },
                    new WorkingPeriod { Id = 5, StartTime = new DateTime(2021, 02, 02, 22, 10, 0), EndTime = new DateTime(2021, 02, 03, 0, 35, 0) }
                }  }
            };
        }

    }
}
