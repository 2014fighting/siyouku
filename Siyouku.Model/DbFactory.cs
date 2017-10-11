using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Siyouku.Model
{
    public  class DbFactory
    {
        private static readonly object SyncRoot=new object();
        private DbFactory()
        {}
        #region 获取当前的上下文对象
        /// <summary>
        /// 单例工厂 获取当前的上下文对象  //考虑多线程情况
        /// </summary>
        /// <returns></returns>
        public static  SiyoukuContext GetCurrentDbContext()
        {
            var context = (SiyoukuContext) CallContext.GetData("DbContext"); //从数据槽获取
            if (context == null)
            {
                lock (SyncRoot)//双重锁定
                {
                    if (context == null)
                    {
                        //如果不存在 ,则实例化一个上下文对象
                        context = new SiyoukuContext();
                        CallContext.SetData("DbContext", context);
                    }
                }
            }
            return context;
        }

        #endregion
    }

    //public class Singleton
    //{
    //    private static Singleton _instance;
    //    //程序执行时创建一个静态的只读的进程辅助对象
    //    private static readonly object SyncRoot = new object();


    //    private Singleton()//私有化构造函数，阻止外界利用new关键词来创建实例
    //    {
            
    //    }

    //    public static Singleton GetInstance()
    //    {
    //        if (_instance == null)//先判断实例是否存在，不存在在枷锁处理
    //        {
    //            lock (SyncRoot)//在同一时刻加了锁的那部分程序只有一个线程可以进入
    //            {
    //                _instance = _instance ?? (_instance = new Singleton());
    //            }
    //        }
    //        return _instance;
    //    }
    //}

    //public class Mainclass
    //{
    //    public void Myaction()
    //    {
    //        var s1 = Singleton.GetInstance();
    //        var s2 = Singleton.GetInstance();
    //        if (s1 == s2)
    //        {
    //            Console.WriteLine("两个对象是相同的！");
    //        }
    //        Console.Read();
    //    }
    //    //单例模式，多线程单例，双重锁定单例，工厂单例创建上下文。
    //    //单例模式：保证一个类仅有一个实例，并提供一个访问它的全局访问点
    //    //http://img.siyouku.cn/shejimoshi.jpg
    //}


}
