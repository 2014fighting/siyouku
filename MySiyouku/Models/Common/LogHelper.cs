using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Repository;

namespace MySiyouku.Models.Common
{
    public class LogHelper
    {
        /// <summary>
        /// The logger
        /// </summary>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        private static readonly ILog Logger;

        static LogHelper()
        {
            if (Logger == null)
            {
                Logger = LogManager.GetLogger(typeof(LogHelper));
                InitConfig();
            }
        }

        /// <summary>
        /// Log开始标记
        /// </summary>
        public static string LogTextStart = "===开始===";
        /// <summary>
        /// Log结束标记
        /// </summary>
        public static string LogTextEnd = "===结束===\r\n";
        /// <summary>
        /// Errors the specified ex.
        /// </summary>   
        /// <param name="ex">Error Message.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Error(Exception ex)
        {
            Logger.Error(ex);
            ILoggerRepository rep = LogManager.GetRepository();
            foreach (IAppender appender in rep.GetAppenders())
            {
                var buffered = appender as BufferingAppenderSkeleton;
                buffered?.Flush();
            }
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="ex">The ex.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Error(string message, Exception ex)
        {
            Logger.Error("Method Name:" + MethodBase.GetCurrentMethod().Name + "\r\n" + message, ex);
        }

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Info(string message)
        {
            Logger.Info(message);
        }

        /// <summary>
        /// Debugs the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Debug(Exception ex)
        {
            Logger.Debug(ex);
        }

        /// <summary>
        /// Debugs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Debug(String message)
        {
            Logger.Debug(message);
        }

        /// <summary>
        /// Warns the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void Warn(Exception ex)
        {
            Logger.Warn(ex);
        }

        /// <summary>
        /// Initializes the configuration. Path：bin/log4net.Config
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void InitConfig(string filePath)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(filePath));
        }
        /// <summary>
        /// 初始化Log配置,首取web.config的"LogConfigFile"设定的路径,若不存在,则取Bin\\log4net.Config
        /// </summary>
        /// <history>
        /// 1. wenqing, 2017年7月11日, Created
        /// </history>
        public static void InitConfig()
        {
            //取设定路径
            string filePath = System.Configuration.ConfigurationManager.AppSettings["LogConfigFile"];
            //若没有设定路径,则取Bin\\log4net.Config
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = AppDomain.CurrentDomain.BaseDirectory + "Bin\\log4net.Config";
            }
            else
            {
                if (filePath.IndexOf(":", StringComparison.Ordinal) < 0)
                {
                    filePath = AppDomain.CurrentDomain.BaseDirectory + filePath;
                }
            }
            //初始化Log
            InitConfig(filePath);
        }
    }
}