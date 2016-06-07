using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace rudistor
{

    #region INI结构介绍
    /**
    INI文件其实是一种具有特定结构的文本文件，它的构成分为三部分，结构如下：
    [Section1]
    key 1 = value2
    key 1 = value2
    ……
    [Section2]
    key 1 = value1
    key 2 = value2
    ……
 
    文件由若干个段落（section）组成，每个段落又分成若干个键（key）和值（value）。
    */
    #endregion

    #region INI读写辅助类，采用WIN32API

    /*
    Windows系统自带的Win32的API函数GetPrivateProfileString()和WritePrivateProfileString()分别实现了对INI文件的读写操作，他们位于kernel32.dll下。
    但是令人遗憾的是C#所使用的.NET框架下的公共类库并没有提供直接操作INI文件的类，所以唯一比较理想的方法就是调用API函数。
    然后，.Net框架下的类库是基于托管代码的，而API函数是基于非托管代码的，（在运行库的控制下执行的代码称作托管代码。相反，在运行库之外运行的代码称作非托管代码。）如何实现托管代码与非托管代码之间的操作呢？.Net框架的System.Runtime.InteropServices命名空间下提供各种各样支持COM interop及平台调用服务的成员，其中最重要的属性之一DllImportAttribute可以用来定义用于访问非托管API的平台调用方法，它提供了对从非托管DLL导出的函数进行调用所必需的信息。下面就来看一下如何实现C#与API函数的互操作。 
    读操作：
    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath); 
    section：要读取的段落名
    key: 要读取的键
    defVal: 读取异常的情况下的缺省值
    retVal: key所对应的值，如果该key不存在则返回空值
    size: 值允许的大小
    filePath: INI文件的完整路径和文件名写操作：
 
    写操作
    [DllImport("kernel32")] 
    private static extern long WritePrivateProfileString(string section, string key, string val, string filePath); 
    section: 要写入的段落名
    key: 要写入的键，如果该key存在则覆盖写入
    val: key所对应的值
    filePath: INI文件的完整路径和文件名
    */
    #endregion
    public class INIManager
    {
        public string iniPath; //INI文件路径以及名称

        #region DLL导入
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion

        #region 构造函数
        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="INIPath">文件名</param> 
        public INIManager(string ININame)
        {
            iniPath = Environment.CurrentDirectory + "\\" + ININame;
        }

        /// <summary>
        /// 在不提供文件时候的构造方法
        /// </summary>
        public INIManager() //如果不提供INI文件的路径以及名称，则默认为当前的应用名称.ini
        {
            char[] charsToTrim = { '\"', ' ' };
            string fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(Environment.CommandLine.Trim(charsToTrim));
            iniPath = Environment.CurrentDirectory + "\\" + fileNameWithoutExtension + ".ini";
            //Console.WriteLine(iniPath);  //Test Example
        }
        #endregion

        #region INI写入
        /// <summary> 
        /// 写入INI文件，如果键，值存在则直接覆盖
        /// </summary> 
        /// <param name="section">段落名称(如"SectionName"(无需双引号）)</param> 
        /// <param name="key">键</param> 
        /// <param name="value">值</param> 
        public void IniWriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, this.iniPath);
        }

        /// <summary>
        /// 写入INI文件，默认Default段落
        /// </summary>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public void IniWriteValue(string Key, string Value)
        {
            string Section = @"Default";
            IniWriteValue(Section, Key, Value);
        }
        #endregion

        #region INI读取
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="section">段落名称(如"SectionName"(无需双引号） )</param> 
        /// <param name="Key">键</param> 
        /// <returns>返回对应键的值</returns>
        public string IniReadValue(string section, string Key)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(section, Key, "", temp, 500, this.iniPath);
            return temp.ToString();
        }

        /// <summary>
        /// 读出INI文件,默认情况下从Default中读取
        /// </summary>
        /// <param name="Key">键</param>
        /// <returns>返回对应键的值</returns>
        public string IniReadValue(string Key)
        {
            string section = @"Default";
            return IniReadValue(section, Key);
        }


        #endregion


    }
}
