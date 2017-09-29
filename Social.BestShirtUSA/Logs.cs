using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{

    class Logs
    {
        public void ILogs(string text)
        {
            using (FileStream fs = new FileStream("C:\\Sunfrog\\Results.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(text);
            }
        }

        public void IErrors(string text)
        {
            using (FileStream fs = new FileStream("C:\\Sunfrog\\Errors.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.WriteLine(text);
            }
        }
    }
}
