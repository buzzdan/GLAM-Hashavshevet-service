using GlamServer.DAL;
using GlamServer.Forcast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace Glam.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private string connectionString = @"Data Source=DAN\SQLEXPRESS;Initial Catalog=Glam;Persist Security Info=True;User ID=sa;Password=p@ssword1";

        [TestMethod]
        public void TestMoed()
        {
            ForcastService forcast = new ForcastService(connectionString);
            var table = forcast.RunForcastMoed(holidayId: 1);
        }

        [TestMethod]
        public void TestProduct()
        {
            ForcastService forcast = new ForcastService(connectionString);
            DataTable table = forcast.RunForcastProduct(productId: 1001);
        }

        [TestMethod]
        public void TestProductDate()
        {
            ForcastService forcast = new ForcastService(connectionString);
            var start = new DateTime(2014, 1, 1);
            var end = new DateTime(2014, 12, 1);
            var productId = 1001;
            DataTable table = forcast.RunForcastProductDates(productId, start, end);
        }

        [TestMethod]
        public void Double()
        {
            double r = 0.0;
            string s = r.ToString();
            r = 7.5;
            s = r.ToString();
        }
    }
}