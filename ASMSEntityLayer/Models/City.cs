﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASMSEntityLayer.Models
{
    [Table("Cities")]
    public class City:Base<byte>
    {
        [Required]
        [StringLength(50, MinimumLength=2, ErrorMessage = "İl adı en az 2 en çok 50 karakter aralığında olmalıdır.")]

        public string CityName { get; set; }
        [Required]
        //TODO isUnique core da nasıl yapılır ?

        public byte PlateCode { get; set; }

        //ilişkiler kurulacak
        public virtual ICollection<District> Districts { get; set; }

        //Yukarıdakini kullanırsak .ToList() yapmak gerekiyor
        //Eriniyorsak .ToList() yapmaya aşağıdakini kullanırız
        
        //public virtual List<District>Districts{get;set;}
    }
}
