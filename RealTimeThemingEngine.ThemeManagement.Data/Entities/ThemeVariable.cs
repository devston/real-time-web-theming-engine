using System.Collections.Generic;

namespace RealTimeThemingEngine.ThemeManagement.Data.Entities
{
    public class ThemeVariable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThemeVariable()
        {
            this.ThemeVariableValues = new HashSet<ThemeVariableValue>();
        }

        public int ThemeVariableId { get; set; }
        public int VariableTypeId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }

        public virtual ThemeVariableCategory ThemeVariableCategory { get; set; }
        public virtual ThemeVariableType ThemeVariableType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThemeVariableValue> ThemeVariableValues { get; set; }
    }
}
