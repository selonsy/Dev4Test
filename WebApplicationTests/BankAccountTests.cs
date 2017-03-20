using NUnit.Framework;
using WebApplication.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.Controllers.Tests
{
    [TestFixture()]
    public class BankAccountTests
    {
        /// <summary>
        /// 取款测试
        /// </summary>
        [Test()]
        public void DebitTest()
        {            
            //创建一个账户devin，余额为100元
            BankAccount bank = new BankAccount("devin", 100);
            //取款10元
            bank.Debit(10);

            double expected = 90;
            double actual = bank.Balance;

            //Assert在这里可以理解成断言：在VS里做单元测试是基于断言的测试
            //预计的结果是90元，如果等于实际的结果的话就通过，否则不通过。
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// 存款测试
        /// </summary>
        [Test()]
        public void CreditTest()
        {
            //创建一个账户devin，余额为100元
            BankAccount bank = new BankAccount("devin", 100);
            //存款10元
            bank.Credit(10);
            //Assert在这里可以理解成断言：在VS里做单元测试是基于断言的测试
            //预计的结果是110元，如果等于实际的结果的话就通过，否则不通过。
            Assert.AreEqual(110, bank.Balance);
        }
        
        /// <summary>
        /// 测试Debit方法，如果取款金额小于0，那么应该抛出超限的异常
        /// </summary>
        [Test()]        
        public void Debit_WhenAmountIsLessThanZero_ShouldThrowArgumentOutOfRange()
        {
            // arrange
            double beginningBalance = 11.99;
            double debitAmount = -100.00;
            BankAccount account = new BankAccount("devin", beginningBalance);

            // act
            try
            {
                account.Debit(debitAmount);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }

            //如果不进入catch的话，在不检查任何条件的情况下使断言失败，显示消息
            Assert.Fail("No exception was thrown.");
        }
        
        /// <summary>
        /// 测试Debit方法，如果金额大于余额，那么应该抛出超限的异常
        /// </summary>
        [Test()]
        public void Debit_WhenAmountIsGreaterThanBalance_ShouldThrowArgumentOutOfRange()
        {
            // arrange
            double beginningBalance = 21.99;
            double debitAmount = 22.0;
            BankAccount account = new BankAccount("devin", beginningBalance);

            // act
            try
            {
                account.Debit(debitAmount);
            }
            catch (ArgumentOutOfRangeException e)
            {
                // assert
                StringAssert.Contains(e.Message, BankAccount.DebitAmountExceedsBalanceMessage);
                return;
            }

            //如果不进入catch的话，在不检查任何条件的情况下使断言失败，显示消息
            Assert.Fail("No exception was thrown.");
        }
    }
}