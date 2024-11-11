using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace L072NS_HSZF_2024251.Model
{
    public class Region
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegionNumber { get; set; }
        [Required]
        public string RegionName { get; set; }
        

        public virtual ICollection<Route> Routes { get; set; }
    }
}
