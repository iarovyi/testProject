using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using MyApp.DAL;
using MyApp.Domain;
using System.Linq.Dynamic;

namespace MyApp.ConsoleApp
{

    public class CustomerDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        // etc.
    }

    public class CustomerSearchCriteriaDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedDate { get; set; }
        // etc.
    }

    public class SortingPagingDto
    {
        public SortOrderDto[] SortOrders { get; set; }
        public int PageNumber { get; set; }
        public int NumberRecords { get; set; }
    }
    public class SortOrderDto
    {
        public enum SortOrder
        {
            Ascending,
            Descending
        }
        public string ColumnName { get; set; }
        public SortOrder ColumnOrder { get; set; }
    }

    public static class MyExtensions
    {
        public static T[] ApplySortingPaging<T>(this IOrderedQueryable<T> query, SortingPagingDto sortingPaging)
        {
            var firstPass = true;
            foreach (var sortOrder in sortingPaging.SortOrders)
            {
                if (firstPass)
                {
                    firstPass = false;
                    query = sortOrder.ColumnOrder == SortOrderDto.SortOrder.Ascending
                                ? query.OrderBy(sortOrder.ColumnName) :
                                  query.OrderByDescending(sortOrder.ColumnName);
                }
                else
                {
                    query = sortOrder.ColumnOrder == SortOrderDto.SortOrder.Ascending
                                ? query.ThenBy(sortOrder.ColumnName) :
                                  query.ThenByDescending(sortOrder.ColumnName);
                }
            }

            var result = query.Skip((sortingPaging.PageNumber - 1) *
              sortingPaging.NumberRecords).Take(sortingPaging.NumberRecords).ToArray();

            return result;
        }

        public CustomerDto[] SelectCustomer(CustomerSearchCriteriaDto searchCriteria,
                     SortingPagingDto sortingPaging)
        {
            using (var context = new MyContext())
            {
                var predicate = ExpressionExtensions.BuildPredicate<Customer,
                                       CustomerSearchCriteriaDto>(searchCriteria);

                var query = context.Customers.AsExpandable().Where(predicate)
                                    as IOrderedQueryable<Customer>;

                var dbCustomers = query.ApplySortingPaging(sortingPaging);

                var customerDtos = dbCustomers.ToDto();

                return customerDtos;
            }
        }
    }
    


    public static class DynamicExtensions
    {
        private static readonly string _external = "foo";
        public static IQueryable<T> DynamicLinqQuery<T>(this IDbSet<T> set, string linqQuery) where T : class
        {
            IQueryable<T> queryableData = set.AsQueryable<T>();
            var externals = new Dictionary<string, object>();
            externals.Add(_external, queryableData);
            var expression = System.Linq.Dynamic.DynamicExpression.Parse(typeof(IQueryable<T>), _external + "." + linqQuery, new[] { externals });
            return queryableData.Provider.CreateQuery<T>(expression);
        }
    }

    //Contains - http://blog.walteralmeida.com/2010/05/advanced-linq-dynamic-linq-library-add-support-for-contains-extension-.html
    // !!!!!!!!!!!! http://www.codeproject.com/Articles/493917/Dynamic-Querying-with-LINQ-to-Entities-and-Express
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MyAppContext())
            {
                //AddPrograms(context);
                //var dbCustomers = query.ApplySortingPaging(sortingPaging);

                var fd = context.PeriodPrograms.Where("Name.Contains(@0)", new object[] { "Num2", "Num3" }).ToList();
                var f = context.PeriodPrograms.Where("Name.Contains(@0)", "Num2").ToList();
                var fff2 = context.PeriodPrograms.DynamicLinqQuery("Where(p => (p.Id = 4 And p.Id > 2)).OrderBy(p=>(p.Name)).Select(p=>(p))").ToList();

                var whereId2 = context.PeriodPrograms.Where("Id = 2").ToList();
                var orderedByName = context.PeriodPrograms.OrderBy("Name").ToList();
                var sqlQueryRes =  context.PeriodPrograms.SqlQuery("SELECT * FROM [dbo].[PeriodPrograms]").ToList();
            }
        }

        #region Seed Data
        private static void AddPrograms(MyAppContext context)
        {
            for (int i = 0; i < 10; i++)
            {
                AddProgram(context, "programNum" + i);
            }
        }

        private static void AddProgram(MyAppContext context, string program)
        {
            context.PeriodPrograms.Add(new PeriodProgram()
            {
                Name = program,
                OC = new OC(){ Name = "OC" + program },
                PeriodCosts = new List<PeriodCost>()
                {
                    new PeriodCost() { FullName = "hello"},
                    new PeriodCost() { FullName = "hello"},
                    new PeriodCost() { FullName = "hello"}
                }
            });

            context.SaveChanges();
        }
        #endregion
    }
}
