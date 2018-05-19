using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Data.Entities
{
    public class Theme
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Theme()
        {
            this.ThemeVariableValues = new HashSet<ThemeVariableValue>();
        }

        public int ThemeId { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool ReadOnly { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemeVariableValue> ThemeVariableValues { get; set; }
    }
}
