﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASMSDataAccessLayer.ContractsDAL;
using ASMSDataAccessLayer.ImplementationsDAL;
using ASMSEntityLayer.Models;


namespace ASMSDataAccessLayer.ImplementationsDAL
{
    public class StudentRepo : RepositoryBase<Student, string>, IStudentRepo
    {
        public StudentRepo(MyContext myContext) : base(myContext)
        {

        }
    }
    
}
