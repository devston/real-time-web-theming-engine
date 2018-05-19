using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Data.Entities
{
    public class ThemeVariableType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThemeVariableType()
        {
            this.ThemeVariables = new HashSet<ThemeVariable>();
        }

        public int ThemeVariableTypeId { get; set; }
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemeVariable> ThemeVariables { get; set; }
    }
}
