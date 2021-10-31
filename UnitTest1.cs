using Microsoft.VisualStudio.TestTools.UnitTesting;
using LabCalculator1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LabCalculator1.Tests
{
    [TestClass]
    public class Form1Tests
    {
        [TestMethod()]
        public void Form1Test()
        {
            Form1 form = new Form1();

            
            Assert.AreEqual(form.getDataGridView().Columns[0].Name, "A");
            Assert.AreEqual(form.getDataGridView().Columns[4].Name, "A4");
            
            
            Assert.AreEqual(form.getDataGridView().Rows[0].HeaderCell.Value, "01");
            Assert.AreEqual(form.getDataGridView().Rows[4].HeaderCell.Value, "04");
        }
        [TestMethod()]
       
        public void TestMethod1()
        {
        }
    }
}
