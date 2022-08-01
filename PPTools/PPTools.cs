using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace PPTools
{
    class PPTools
    {
        private Dictionary<string,string> commandFileMap = new Dictionary<string,string>()
        {
            {"getImageData", "图像数据读取\\图像数据读取.exe"},
            {"imageStitch", "图像拼接\\图像拼接.exe"},
            {"Subgraphextraction", "子图提取\\子图提取.exe"},
            {"RemoveEdge", "边框处理\\边框处理.exe" },
            {"LosslessAmplification", "无损放大\\无损放大.exe" },
            {"HandwritingExtraction", "笔迹提取\\笔迹提取.exe" }
            //其他功能文件
        };

        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                MessageBox.Show("没有处理的对象");
                return;
            }

            PPTools tool = new PPTools();
            string command = (string)args.GetValue(args.Length - 1);
            //MessageBox.Show(command);
            List<string>paths =  args.Take(args.Length - 1).ToList();

            List<string> argPaths = new List<string>();
            foreach (string path in paths)
            {
                string argPath = string.Format("\"{0}\"", path);
                argPaths.Add(argPath);
            }

            string arg = string.Join(" ", argPaths);
            tool.execCommand(command,arg);

            return;
        }

        /// <summary>
        /// 根据命令选择合适的功能执行
        /// </summary>
        /// <param name="command">处理图片要使用的功能</param>
        /// <param name="arg">需要处理的对象</param>

        private void execCommand(string command, string arg)
        {
            string rootPath = getRootPath();
            rootPath = rootPath + @"\Tools";  //所有的功能文件都放在当前执行文件的Tools文件夹下。
            foreach(KeyValuePair<string,string> kv in commandFileMap)
            {
                if (Equals(command, kv.Key))
                {
                    string appFile = string.Format(@"{0}\{1}", rootPath, kv.Value);
                    Process.Start(appFile,arg);
                }
            }
        }
        /// <summary>
        /// 获得当前可执行文件PPTools的根目录，所有的图像处理工具都放在当前执行文件的Tools文件夹下。
        /// </summary>
        /// <returns></returns>
        private string getRootPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);   //通过 CodeBase 得到一个 URI 格式的路径；
            string path = Uri.UnescapeDataString(uri.Path); //用 UriBuild.UnescapeDataString 去掉前缀 File://；
            return Path.GetDirectoryName(path);  //用 GetDirectoryName 把它变成正常的 windows 格式。
        }
    }
}
