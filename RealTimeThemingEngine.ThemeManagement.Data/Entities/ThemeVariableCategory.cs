using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Data.Entities
{
    public class ThemeVariableCategory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThemeVariableCategory()
        {
            this.ThemeVariables = new HashSet<ThemeVariable>();
        }

        public int ThemeVariableCategoryId { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemeVariable> ThemeVariables { get; set; }
    }
}
