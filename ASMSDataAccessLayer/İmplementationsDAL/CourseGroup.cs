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
    public class CourseGroupRepo:RepositoryBase<CourseGroup, int>, ICourseGroupRepo
    {
        public CourseGroupRepo(MyContext myContext) : base(myContext)
        {

        }
    }

}
