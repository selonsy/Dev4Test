using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication.Controllers
{
    public class TestController : Controller
    {
        #region MyRegion

        /// <summary>
        /// cookie名称
        /// </summary>
        private static string cookie_name = "mdata";
        /// <summary>
        /// cookie有效时间
        /// </summary>
        private static int cookie_expire = 20 * 60;

        #endregion

        /// <summary>
        /// 获取电影列表
        /// </summary>
        /// <returns></returns>
        public JsonResult List(int type = 0)
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (type == 1 && !_is_login())
            {
                //校验是否登录
                result.Data = new { code = 3, msg = "forbidden", info = "非法访问" };
                return result;
            }
            result.Data = new
            {
                code = 1,
                msg = "success",
                info = "成功",
                data = new[] {
                        new { name="金刚狼3",director="路人甲" },
                        new { name="速度与激情8",director="路人乙"},
                        new { name="生化危机7",director="路人丙"}
                    }
            };
            return result;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns></returns>
        public JsonResult Login()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            string username = Request.QueryString["username"];
            string password = Request.QueryString["password"];
            if (username == "admin" && password == "123456")
            {
                //保存登录信息
                _save_cookie(username, password);
                result.Data = new { code = 1, msg = "success", info = "登录成功" };
            }
            else
            {
                result.Data = new { code = 0, msg = "fail", info = "登录失败" };
            }
            return result;
        }

        /// <summary>
        /// Fiddler测试
        /// </summary>
        /// <returns></returns>
        public ActionResult Fiddler()
        {
            return View();
        }
        #region private

        /// <summary>
        /// 保存cookie
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        private void _save_cookie(string username, string password)
        {
            HttpCookie cookie = Request.Cookies[cookie_name];
            if (cookie == null)
                cookie = new HttpCookie(cookie_name);
            cookie.Value = username + ":" + password;
            cookie.Expires = DateTime.Now.AddSeconds(cookie_expire);
            Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 校验cookie
        /// </summary>
        private bool _is_login()
        {
            bool result = false;
            HttpCookie cookie = Request.Cookies[cookie_name];
            if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
            {
                string[] cookie_values = cookie.Value.Split(':');
                if (cookie_values.Length == 2 && cookie_values[0] == "admin" && cookie_values[1] == "123456")
                {
                    result = true;
                }
            }
            return result;
        }

        #endregion
    }

    /// <summary> 
    /// Bank Account demo class. 
    /// </summary> 
    public class BankAccount
    {
        #region 字段

        //用户名
        private readonly string _customerName;

        //账户余额
        private double _balance;

        //账户状态
        private bool _frozen = false;

        //超限异常信息，自定义，用于获取具体抛出的异常
        public const string DebitAmountExceedsBalanceMessage = "Debit amount exceeds balance";
        public const string DebitAmountLessThanZeroMessage = "Debit amount less than zero";

        #endregion

        #region 构造方法

        private BankAccount()
        {
        }

        public BankAccount(string customerName, double balance)
        {
            _customerName = customerName;
            _balance = balance;
        }

        #endregion

        #region 属性

        public string CustomerName
        {
            get { return _customerName; }
        }

        public double Balance
        {
            get { return _balance; }
        }

        #endregion

        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="amount"></param>
        public void Debit(double amount)
        {
            if (_frozen)
            {
                throw new Exception("您的账户被禁用了！");
            }
            if (amount > _balance)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountExceedsBalanceMessage);
            }
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }
            _balance -= amount;
        }

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="amount"></param>
        public void Credit(double amount)
        {
            if (_frozen)
            {
                throw new Exception("您的账户被禁用了！");
            }
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount", amount, DebitAmountLessThanZeroMessage);
            }
            _balance += amount;
        }

        #region private

        /// <summary>
        /// 冻结账户
        /// </summary>
        private void FreezeAccount()
        {
            _frozen = true;
        }

        /// <summary>
        /// 解冻账户
        /// </summary>
        private void UnfreezeAccount()
        {
            _frozen = false;
        }

        #endregion
    }
}