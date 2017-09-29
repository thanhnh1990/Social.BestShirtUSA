using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.BestShirtUSA
{
    class Start
    {
        public void Excute()
        {
            QueryLocal action = new QueryLocal();
            var results = action.GetProduct();
            foreach(var i in results)
            {
              Logic call = new Logic();
                call.execute(i);
            }
        }
    }
}
