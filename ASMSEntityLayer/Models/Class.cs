using ASMSEntityLayer.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.Models
{
    public class Class:Base<int>
    {
        [Required]
        [StringLength(50,MinimumLength =2,ErrorMessage="Sınıf adı en az 2 en çok 50 karakter aralığında olmalıdır!!")]
        public string ClassName { get; set; }
        //Bu enum olacak
        public ClassLacation ClassFloor { get; set; }//Kat 1 gibi ??
        //ilişkinin karşılığı
        public virtual ICollection<CourseGroup> CourseGroups { get; set; }
    }
}
