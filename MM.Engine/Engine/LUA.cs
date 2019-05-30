﻿using NLua;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MM.Engine
{
    /// <summary>
    /// 脚本引擎接口
    /// </summary>
    public class LUA : IEngine
    {
        private readonly string _Dir;

        #region 属性
        /// <summary>
        /// 错误提示
        /// </summary>
        public string Ex { get; set; }

        /// <summary>
        /// 脚本函数字典
        /// </summary>
        internal static ConcurrentDictionary<string, dynamic> dict = new ConcurrentDictionary<string, dynamic>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 脚本函数字典
        /// </summary>
        public ConcurrentDictionary<string, dynamic> Dict
        {
            get { return dict; }
            set { dict = value; }
        }
        #endregion


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dir">脚本目录</param>
        public LUA(string dir = null)
        {
            if (string.IsNullOrEmpty(dir))
            {
                _Dir = Cache.runPath;
            }
            else
            {
                _Dir = dir;
            }
        }

        /// <summary>
        /// 获取错误信息
        /// </summary>
        /// <returns>返回错误信息</returns>
        public string GetEx()
        {
            return Ex;
        }

        /// <summary>
        /// 遍历加载
        /// </summary>
        /// <param name="fileList">应用列表</param>
        /// <returns>加载成功返回true，失败返回false</returns>
        public bool EachLoad(List<string> fileList)
        {
            var bl = true;
            foreach (var file in fileList)
            {
                bl = Load(file);
                if (!bl)
                {
                    break;
                }
            }
            return bl;
        }

        /// <summary>
        /// 载入脚本
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>载入成功返回true，失败返回false</returns>
        public bool Load(string file)
        {
            var bl = false;
            file = file.ToFullName(_Dir);
            var dyn = GetClass(file);
            if (dyn != null)
            {
                var key = file.Replace(Cache.runPath, "");
                key = key.ToLower();
                if (dict.ContainsKey(key))
                {
                    dict[key] = dyn;
                    bl = true;
                }
                else
                {
                    bl = dict.TryAdd(key, dyn);
                }
            }
            return bl;
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns>卸载成功返回true，失败返回false</returns>
        public bool Unload(string appName)
        {
            if (!string.IsNullOrEmpty(appName))
            {
                return dict.TryRemove(appName.ToFullName(_Dir).Replace(Cache.runPath, ""), out _);
            }
            return false;
        }

        /// <summary>
        /// 卸载脚本
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="waitTime">等待时长，单位：毫秒</param>
        /// <returns>卸载成功返回true，失败返回false</returns>
        public void Unload(string appName, int waitTime)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(waitTime);
                Unload(appName);
            });
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object Run(string appName, object fun, object param1 = null, object param2 = null, object param3 = null)
        {
            var key = appName.ToFullName(_Dir).Replace(Cache.runPath, "");
            if (!dict.ContainsKey(key))
            {
                var bl = Load(appName);
                if (!bl)
                {
                    return null;
                }
            }
            if (dict.TryGetValue(key, out dynamic dyn))
            {
                if (dyn != null)
                {
                    try
                    {
                        if (param1 == null)
                        {
                            return dyn.Main(fun);
                        }
                        else if (param2 == null)
                        {
                            return dyn.Main(fun, param1);
                        }
                        else if (param3 == null)
                        {
                            return dyn.Main(fun, param1, param2);
                        }
                        else
                        {
                            return dyn.Main(fun, param1, param2, param3);
                        }
                    }
                    catch (Exception ex)
                    {
                        Ex = ex.ToString();
                    }
                }
            }
            else
            {
                Ex = "程序未加载！";
            }
            return null;
        }

        /// <summary>
        /// 执行脚本
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public Task RunAsync(string appName, object fun, object param1 = null, object param2 = null, object param3 = null)
        {
            return Task.Run(() => Run(appName, fun, param1, param2, param3));
        }

        /// <summary>
        /// 执行脚本文件
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object RunFile(string file, object fun, object param1 = null, object param2 = null, object param3 = null)
        {
            if (string.IsNullOrEmpty(file))
            {
                Ex = "脚本文件名不能为空";
                return null;
            }
            file = file.ToFullName(_Dir);
            if (!File.Exists(file))
            {
                Ex = "脚本不存在！请确认脚本：“" + file + "”是否存在。";
                return null;
            }
            try
            {
                Lua Eng = new Lua();
                Eng.LoadCLRPackage();
                var dir = file.ToDir();
                Eng["SDK"] = new Helper.Indexer() { Dir = dir };
                Eng["Cache"] = new Cache();
                Eng["Engine"] = new Indexer() { Dir = dir };
                Eng.DoFile(file);
                var funObj = Eng.GetFunction("Main");
                if (param1 == null)
                {
                    return funObj.Call(fun)[0];
                }
                else if (param2 == null)
                {
                    return funObj.Call(fun, param1)[0];
                }
                else if (param3 == null)
                {
                    return funObj.Call(fun, param1, param2)[0];
                }
                else
                {
                    return funObj.Call(fun, param1, param2, param3)[0];
                }
            }
            catch (Exception ex)
            {
                Ex = ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 执行脚本代码
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="fun">函数名</param>
        /// <param name="param1">参数1</param>
        /// <param name="param2">参数2</param>
        /// <param name="param3">参数3</param>
        /// <returns>返回执行结果</returns>
        public object RunCode(string code, object fun, object param1 = null, object param2 = null, object param3 = null)
        {
            if (string.IsNullOrEmpty(code))
            {
                Ex = "脚本不能为空";
                return null;
            }
            try
            {
                Lua Eng = new Lua();
                Eng.LoadCLRPackage();
                var dir = _Dir;
                Eng["SDK"] = new Helper.Indexer() { Dir = dir };
                Eng["Cache"] = new Cache();
                Eng["Engine"] = new Indexer() { Dir = dir };
                Eng.DoString(code, "Script");
                var funObj = Eng.GetFunction("Main");
                if (param1 == null)
                {
                    return funObj.Call(fun)[0];
                }
                else if (param2 == null)
                {
                    return funObj.Call(fun, param1)[0];
                }
                else if (param3 == null)
                {
                    return funObj.Call(fun, param1, param2)[0];
                }
                else
                {
                    return funObj.Call(fun, param1, param2, param3)[0];
                }
            }
            catch (Exception ex)
            {
                Ex = ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取函数
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>返回函数</returns>
        public dynamic GetClass(string file)
        {
            if (string.IsNullOrEmpty(file))
            {
                Ex = "脚本文件名不能为空";
                return null;
            }
            file = file.ToFullName(_Dir);
            if (!File.Exists(file))
            {
                Ex = "脚本不存在！请确认脚本：“" + file + "”是否存在。";
                return null;
            }
            try
            {
                Lua Eng = new Lua();
                Eng.LoadCLRPackage();
                var dir = file.ToDir();
                Eng["SDK"] = new Helper.Indexer() { Dir = dir };
                Eng["Cache"] = new Cache();
                Eng["Engine"] = new Indexer() { Dir = dir };
                var code = File.ReadAllText(file, Encoding.UTF8);
                Eng.DoFile(file);
                return Eng.Pop();
            }
            catch (Exception ex)
            {
                Ex = ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取类
        /// </summary>
        /// <param name="appName">应用名</param>
        /// <returns>返回实例化类</returns>
        public dynamic Get(string appName)
        {
            dict.TryGetValue(appName, out dynamic dyn);
            return dyn;
        }
    }
}
