using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMSDataAccessLayer.ContractsDAL;
using ASMSDataAccessLayer.İmplementationsDAL;
using ASMSEntityLayer.Models;


namespace ASMSDataAccessLayer.İmplementationsDAL
{
   public class NeighbourhoodRepo : RepositoryBase<Neighbourhood, int>, INeighbourhoodRepo
    {
        public NeighbourhoodRepo(MyContext myContext) : base(myContext)
        {

        }
    }
    
}
