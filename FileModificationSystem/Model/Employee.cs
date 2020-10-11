using DataUtility.Infrastructure.Model;

namespace FileModificationSystem.Model
{
    public class Employee : Entity
    {
        /// <summary>
        /// Gets or Sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets the Age.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// Gets or Sets the Designation.
        /// </summary>
        public string Designation { get; set; }

        /// <summary>
        /// Gets or Sets the Address details. 
        /// </summary>
        public virtual Address Address { get; set; }

        /// <summary>
        /// Gets or Sets the Qualification details.
        /// </summary>
        public virtual Qualification Qualification { get; set; }
    }

    public class Address
    {
        /// <summary>
        /// Gets or Sets the city.
        /// </summary>
        public string City { get; set; }
    }

    public class Qualification
    {
        /// <summary>
        /// Gets or Sets the graduation
        /// </summary>
        public string Graduation { get; set; }
    }
}
