using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDo.DataAccessModels.BaseModels;

namespace ToDo.Tests.Core
{
    public static class QueryableExtensions
    {
        public static Mock<DbSet<T>> GetMockDbSet<T>(this IQueryable<T> data) where T : BaseEntity
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IAsyncEnumerable<T>>().Setup(x => x.GetEnumerator()).Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));
            mockSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(new TestAsyncQueryProvider<T>(data.Provider));
            mockSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());
            return mockSet;
        }
    }
}
